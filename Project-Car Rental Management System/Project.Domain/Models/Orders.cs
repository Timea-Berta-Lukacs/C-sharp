using CSharp.Choices;

namespace Project.Domain.Models
{
    [AsChoice]
    public static partial class Orders
    {
        public interface IOrders { }

        public record UnvalidatedPlacedOrders : IOrders
        {
            public UnvalidatedPlacedOrders(UnvalidatedOrder orderList)
            {
                OrderList = orderList;
            }

            public UnvalidatedOrder OrderList { get; }
        }

        public record InvalidOrders : IOrders
        {
            internal InvalidOrders(UnvalidatedOrder orderList, string reason)
            {
                OrderList = orderList;
                Reason = reason;
            }
            public UnvalidatedOrder OrderList { get; }
            public string Reason { get; }
        }
        public record FailedOrders : IOrders
        {
            internal FailedOrders(UnvalidatedOrder orderList, Exception exception)
            {
                OrderList = orderList;
                Exception = exception;
            }
            public UnvalidatedOrder OrderList { get; }
            public Exception Exception { get; }
        }
        public record ValidatedOrders : IOrders
        {
            public ValidatedOrders(EvaluatedOrder orderList)
            {
                Order = orderList;
            }

            public EvaluatedOrder Order { get; }
        }

        public record PlacedOrders : IOrders
        {
            internal PlacedOrders(EvaluatedOrder orderList, DateTime placedOrderDate)
            {
                OrderList = orderList;
                Date = placedOrderDate;
            }
            public EvaluatedOrder OrderList { get; }
            public DateTime Date { get; }
        }
    }
}