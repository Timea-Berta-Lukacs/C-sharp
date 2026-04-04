using Project.Domain.Repositories;
using Microsoft.Extensions.Logging;
using static Project.Domain.WorkflowEvents.PlaceOrderEvent;
using Project.Domain.Commands;
using Project.Domain.Models;
using LanguageExt;
using static LanguageExt.Prelude;
using static Project.Domain.Operations.PlaceOrderOperation;
using static Project.Domain.Models.Orders;
using Project.Domain.WorkflowEvents;
using System.Text.RegularExpressions;
using LanguageExt.Pipes;
using Fare;

namespace Project.Domain.Workflows
{
    public class PlaceOrderWorkflow
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        private readonly ILogger<PlaceOrderWorkflow> logger;

        public PlaceOrderWorkflow(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository, ILogger<PlaceOrderWorkflow> logger)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public async Task<IPlaceOrderEvent> ExecuteAsync(PlaceOrderCommand command)
        {
            UnvalidatedPlacedOrders unvalidatedOrder = new UnvalidatedPlacedOrders(command.InputOrder);

            var result = from userRegistrationNumbers in userRepository.TryGetExistingUserRegistrationNumbers()
                                    .ToEither(ex => new FailedOrders(unvalidatedOrder.OrderList, ex) as IOrders)
                         from orderNumbers in orderRepository.TryGetExistentOrderNumbers()
                                    .ToEither(ex => new FailedOrders(unvalidatedOrder.OrderList, ex) as IOrders)
                         from existentProducts in productRepository.TryGetExistentProducts()
                                    .ToEither(ex => new FailedOrders(unvalidatedOrder.OrderList, ex) as IOrders)
                         from users in userRepository.TryGetExistingUsers()
                                    .ToEither(ex => new FailedOrders(unvalidatedOrder.OrderList, ex) as IOrders)
                         let checkUserExists = (Func<UserRegistrationNumber, Option<UserRegistrationNumber>>)(user => CheckUserExists(userRegistrationNumbers, user))
                         let checkOrderExists = (Func<OrderNumber, Option<OrderNumber>>)(order => CheckOrderExists(orderNumbers, order))
                         let checkProductsExist = (Func<List<UnvalidatedProduct>, Option<List<EvaluatedProduct>>>)(products => CheckProductsExist(existentProducts, products))
                         let checkUserPaymentDetails = (Func<UnvalidatedPlacedOrders, Option<CardDetailsDto>>)(user => CheckUserPaymentDetails(unvalidatedOrder, users))
                         let updateCardDetails = (Func<CardDetailsDto, Option<CardDetailsDto>>)(card => UpdateCardDetails(card))
                         let checkUserBalance = (Func<UnvalidatedPlacedOrders, IEnumerable<EvaluatedProduct>, CardDetailsDto, Option<UnvalidatedPlacedOrders>>)((unvalidatedOrder, products, cardDetails) => CheckUserBalance(users, unvalidatedOrder, products, cardDetails))
                         from placedOrder in ExecuteWorkflowAsync(unvalidatedOrder, orderNumbers, checkUserExists, checkOrderExists, checkProductsExist, checkUserPaymentDetails, updateCardDetails, checkUserBalance,users).ToAsync()
                         from saveResult in orderRepository.TrySaveOrder(placedOrder)
                                     .ToEither(ex => new FailedOrders(unvalidatedOrder.OrderList, ex) as IOrders)

                         let successfulEvent = new PlaceOrderSucceededEvent(placedOrder.Order, DateTime.Now)
                         select successfulEvent;

            return await result.Match(
                Left: order => GenerateFailedEvent(order) as IPlaceOrderEvent,
                Right: order => order);
        }

        private PlaceOrderFailedEvent GenerateFailedEvent(IOrders order) =>
          order.Match<PlaceOrderFailedEvent>(
          unvalidatedPlacedOrder => new($"Invalid state {nameof(UnvalidatedPlacedOrders)}"),
          invalidOrder => new(invalidOrder.Reason),
          failedOrder =>
          {
              logger.LogError(failedOrder.Exception, failedOrder.Exception.Message);
              return new(failedOrder.Exception.Message);
          },
          validatedOrder => new($"Invalid state {nameof(ValidatedOrders)}"),
          placedOrder => new($"Invalid state {nameof(PlacedOrders)}")
          );

        private async Task<Either<IOrders, ValidatedOrders>> ExecuteWorkflowAsync(UnvalidatedPlacedOrders unvalidatedPlacedOrder,
                                                                             IEnumerable<OrderNumber> orderNumbers,
                                                                             Func<UserRegistrationNumber, Option<UserRegistrationNumber>> checkUserExists,
                                                                             Func<OrderNumber, Option<OrderNumber>> checkOrderExists,
                                                                             Func<List<UnvalidatedProduct>, Option<List<EvaluatedProduct>>> checkProductsExist,
                                                                             Func<UnvalidatedPlacedOrders, Option<CardDetailsDto>> checkUserPaymentDetails,
                                                                             Func<CardDetailsDto, Option<CardDetailsDto>> updateCardDetails,
                                                                             Func<UnvalidatedPlacedOrders, IEnumerable<EvaluatedProduct>, CardDetailsDto, Option<UnvalidatedPlacedOrders>> checkUserBalance, 
                                                                             IEnumerable<UserDto>users)
        {
            unvalidatedPlacedOrder = GenerateOrderNumber(unvalidatedPlacedOrder, orderNumbers);
            IOrders order = await ValidatePlacedOrder(checkUserExists, checkOrderExists, checkProductsExist, checkUserPaymentDetails, checkUserBalance, unvalidatedPlacedOrder,users);

            order = CalculatePrice(order);

            return order.Match<Either<IOrders, ValidatedOrders>>(
                unvalidatedPlacedOrder => Left(unvalidatedPlacedOrder as IOrders),
                invalidOrder => Left(invalidOrder as IOrders),
                failedOrder => Left(failedOrder as IOrders),
                validatedOrder => Right(validatedOrder),
                placedOrder => Left(placedOrder as IOrders)
            );
        }

        private Option<UserRegistrationNumber> CheckUserExists(IEnumerable<UserRegistrationNumber> users, UserRegistrationNumber userRegistrationNumber)
        {
            if (users.Any(u => u == userRegistrationNumber))
            {
                return Some(userRegistrationNumber);
            }
            else
            {
                return None;
            }
        }

        private Option<List<EvaluatedProduct>> CheckProductsExist(IEnumerable<EvaluatedProduct> existentProducts, IEnumerable<UnvalidatedProduct> products)
        {
            if (products.All(product => existentProducts.Any(existingProduct => existingProduct.ProductName.Value == product.ProductName)) &&
                products.All(product => existentProducts.Any(existingProduct => existingProduct.Quantity.Quantity >= product.Quantity)))
            {
                List<EvaluatedProduct> result = new();
                result = (from unvalidated in products
                          join evaluated in existentProducts on unvalidated.ProductName equals evaluated.ProductName.Value
                          select new
                          {
                              ProductName = unvalidated.ProductName,
                              Quantity = unvalidated.Quantity,
                              Price = evaluated.Price.Price
                          })
                             .ToList()
                             .Select(res => new EvaluatedProduct(
                                 new ProductName(res.ProductName),
                                 new ProductQuantity(res.Quantity),
                                 new ProductPrice(res.Price)))
                             .ToList();

                return Option<List<EvaluatedProduct>>.Some(result);
            }
            else
            {
                return None;
            }
        }

        private Option<OrderNumber> CheckOrderExists(IEnumerable<OrderNumber> orders, OrderNumber orderNumber)
        {
            if (orders.Any(o => o == orderNumber))
            {
                return None;
            }
            else
            {
                return Some(orderNumber);
            }
        }

        private Option<UnvalidatedPlacedOrders> CheckUserBalance(List<UserDto> users, UnvalidatedPlacedOrders unvalidatedPlacedOrder, IEnumerable<EvaluatedProduct> products, CardDetailsDto cardDetails)
        {
            var price = products.Sum(p => p.Price.Price * p.Quantity.Quantity);

            if (!cardDetails.ToUpdate)
            {
                var user = users.FirstOrDefault(u => u.UserRegistrationNumber == unvalidatedPlacedOrder.OrderList.UserRegistrationNumber);
                if (user != null && user.Balance >= price)
                {
                    return Some(unvalidatedPlacedOrder);
                }
            }
            else
            {
                if (cardDetails.Balance >= price)
                {
                    return Some(unvalidatedPlacedOrder);
                }
            }
            return None;
        }

        public static UnvalidatedPlacedOrders GenerateOrderNumber(UnvalidatedPlacedOrders unvalidatedOrder, IEnumerable<OrderNumber> orderNumbers)
        {
           Xeger xeger = new Xeger("^ORD[0-9]{6}$");
            var orderNumber = xeger.Generate();
            while (orderNumbers.Any(n => n.Value == orderNumber))
            {
                orderNumber = xeger.Generate();
            }

            return new UnvalidatedPlacedOrders(
                new UnvalidatedOrder
                (
                    UserRegistrationNumber: unvalidatedOrder.OrderList.UserRegistrationNumber,
                    OrderNumber: orderNumber,
                    OrderPrice: 0,
                    OrderDeliveryAddress: unvalidatedOrder.OrderList.OrderDeliveryAddress,
                    OrderTelephone: unvalidatedOrder.OrderList.OrderTelephone,
                    CardNumber: unvalidatedOrder.OrderList.CardNumber,
                    CVV: unvalidatedOrder.OrderList.CVV,
                    CardExpiryDate: unvalidatedOrder.OrderList.CardExpiryDate,
                    OrderProducts: unvalidatedOrder.OrderList.OrderProducts
                )
                );
        }
        private Option<CardDetailsDto> CheckUserPaymentDetails(UnvalidatedPlacedOrders unvalidatedPlacedOrder, IEnumerable<UserDto> users)
        {
            if (unvalidatedPlacedOrder.OrderList.CardNumber == null && unvalidatedPlacedOrder.OrderList.CVV == null && unvalidatedPlacedOrder.OrderList.CardExpiryDate == null)
            {
                var user = users.FirstOrDefault(u => u.UserRegistrationNumber == unvalidatedPlacedOrder.OrderList.UserRegistrationNumber);
                if (user != null)
                {
                    if (user.CardNumber != null && user.CVV != null && user.CardExpiryDate != null && user.Balance != null)
                    {
                        if ((new Regex("[0-9]{16}")).IsMatch(user.CardNumber) && user.CVV.ToString().Length == 3 && user.CardExpiryDate > DateTime.Now)
                        {
                            return Some(new CardDetailsDto()
                            {
                                ToUpdate = false
                            });
                        }
                    }
                }
            }
            else if (unvalidatedPlacedOrder.OrderList.CardNumber != null && unvalidatedPlacedOrder.OrderList.CVV != null && unvalidatedPlacedOrder.OrderList.CardExpiryDate != null)
            {
                if ((new Regex("[0-9]{16}")).IsMatch(unvalidatedPlacedOrder.OrderList.CardNumber) && unvalidatedPlacedOrder.OrderList.CVV.ToString().Length == 3 && unvalidatedPlacedOrder.OrderList.CardExpiryDate > DateTime.Now)
                {

                    return Some(new CardDetailsDto()
                    {
                        UserRegistrationNumber = unvalidatedPlacedOrder.OrderList.UserRegistrationNumber,
                        CardNumber = unvalidatedPlacedOrder.OrderList.CardNumber,
                        CVV = unvalidatedPlacedOrder.OrderList.CVV,
                        CardExpiryDate = unvalidatedPlacedOrder.OrderList.CardExpiryDate,
                        Balance = new Random().NextDouble() * (7000 - 1000) + 1000,
                        ToUpdate = true
                    });
                }
            }
            return None;
        }
        private Option<CardDetailsDto> UpdateCardDetails(CardDetailsDto cardDetailsDto)
        {
            if (cardDetailsDto.ToUpdate)
            {
                userRepository.UpdateCardDetails(cardDetailsDto);
            }

            return Some(cardDetailsDto);
        }
    }
}
    