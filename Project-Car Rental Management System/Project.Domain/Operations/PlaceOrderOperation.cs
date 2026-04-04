using LanguageExt;
using Project.Domain.Models;
using static Project.Domain.Models.Orders;

namespace Project.Domain.Operations
{
    public static class PlaceOrderOperation
    {
        public static Task<IOrders> ValidatePlacedOrder(Func<UserRegistrationNumber, Option<UserRegistrationNumber>> checkUserExists,
                                                 Func<OrderNumber, Option<OrderNumber>> checkOrderExists,
                                                 Func<List<UnvalidatedProduct>, Option<List<EvaluatedProduct>>> checkProductsExist,
                                                 Func<UnvalidatedPlacedOrders, Option<CardDetailsDto>> checkUserPaymentDetails,
                                                 Func<UnvalidatedPlacedOrders, IEnumerable<EvaluatedProduct>, CardDetailsDto, Option<UnvalidatedPlacedOrders>> checkUserBalance,
                                                 UnvalidatedPlacedOrders order, IEnumerable<UserDto> users) =>
           ValidateOrder(checkUserExists, checkOrderExists, checkProductsExist, checkUserPaymentDetails, checkUserBalance, order, users)
            .MatchAsync(
               Right: validatedOrder => new ValidatedOrders(validatedOrder),
               LeftAsync: errorMessage => Task.FromResult((IOrders)new InvalidOrders(order.OrderList, errorMessage))
               );
        private static EitherAsync<string, EvaluatedOrder> ValidateOrder(Func<UserRegistrationNumber, Option<UserRegistrationNumber>> checkUserExists,
                                                 Func<OrderNumber, Option<OrderNumber>> checkOrderExists,
                                                 Func<List<UnvalidatedProduct>, Option<List<EvaluatedProduct>>> checkProductsExist,
                                                 Func<UnvalidatedPlacedOrders, Option<CardDetailsDto>> checkUserPaymentDetails,
                                                 Func<UnvalidatedPlacedOrders, IEnumerable<EvaluatedProduct>, CardDetailsDto, Option<UnvalidatedPlacedOrders>> checkUserBalance,
                                                 UnvalidatedPlacedOrders unvalidatedOrder,
                                                 IEnumerable<UserDto> users) =>
            from userRegistrationNumber in UserRegistrationNumber.TryParse(unvalidatedOrder.OrderList.UserRegistrationNumber)
                                    .ToEitherAsync($"Invalid user registration number ({unvalidatedOrder.OrderList.UserRegistrationNumber}).")
            from orderNumber in OrderNumber.TryParse(unvalidatedOrder.OrderList.OrderNumber)
                                    .ToEitherAsync($"Invalid order number ({unvalidatedOrder.OrderList.OrderNumber}).")
            from userExists in checkUserExists(userRegistrationNumber)
                                    .ToEitherAsync($"User ({unvalidatedOrder.OrderList.UserRegistrationNumber} does not exists).")
            from orderExists in checkOrderExists(orderNumber)
                                    .ToEitherAsync($"Order with order number ({unvalidatedOrder.OrderList.OrderNumber}) already exists.")
            from productsExist in checkProductsExist(unvalidatedOrder.OrderList.OrderProducts)
                                    .ToEitherAsync($"Invalid product list for order ({unvalidatedOrder.OrderList}).")
            from validTelephone in OrderTelephone.TryParse(unvalidatedOrder.OrderList.OrderTelephone)
                                    .ToEitherAsync($"Invalid telephone number ({unvalidatedOrder.OrderList.OrderTelephone})")
            from checkedUserPaymentDetails in checkUserPaymentDetails(unvalidatedOrder)
                                    .ToEitherAsync("Invalid or missing payment details.")
            from checkedBalance in checkUserBalance(unvalidatedOrder, productsExist, checkedUserPaymentDetails)
                                    .ToEitherAsync($"Insufficient funds for paying order ({unvalidatedOrder.OrderList}).")
            select new EvaluatedOrder(orderNumber, new OrderPrice(0), new OrderDeliveryAddress(unvalidatedOrder.OrderList.OrderDeliveryAddress), new OrderTelephone(unvalidatedOrder.OrderList.OrderTelephone), new OrderProducts(productsExist))
            {
                UserRegistrationNumber = userRegistrationNumber,
                CardDetails = new CardDetails(
                    new UserCardNumber(checkedUserPaymentDetails.CardNumber),
                    new UserCardCVV(checkedUserPaymentDetails.CVV),
                    new UserCardExpiryDate(checkedUserPaymentDetails.CardExpiryDate),
                    new UserCardBalance(checkedUserPaymentDetails.Balance),
                    checkedUserPaymentDetails.ToUpdate
                    )
            };


        public static IOrders CalculatePrice(IOrders order) => order.Match(
           unvalidatedPlacedOrder => unvalidatedPlacedOrder,
               invalidOrder => invalidOrder,
               failedOrder => failedOrder,
               validatedOrder =>
               {
                   return new ValidatedOrders(
                       new EvaluatedOrder(
                            validatedOrder.Order.OrderNumber,
                            new OrderPrice(validatedOrder.Order.OrderProducts.OrderProductsList.Sum(p => p.Price.Price * p.Quantity.Quantity)),
                            validatedOrder.Order.OrderDeliveryAddress,
                            validatedOrder.Order.OrderTelephone,
                            validatedOrder.Order.OrderProducts
                            )
                       {
                           UserRegistrationNumber = validatedOrder.Order.UserRegistrationNumber,
                           CardDetails = validatedOrder.Order.CardDetails
                       }
                       );
               },
               placedOrder => placedOrder
           );
    }
}