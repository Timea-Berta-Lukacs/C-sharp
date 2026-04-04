using LanguageExt;
using Project.Domain.Models;
using static Project.Domain.Models.Orders;
using static Project.Domain.Models.ReturnOrders;

namespace Project.Domain.Repositories
{
    public interface IOrderRepository
    {
        TryAsync<List<EvaluatedOrder>> TryGetExistentOrders();
        TryAsync<OrderNumber> TryGetExistentOrder(string orderNumberToCheck);
        TryAsync<List<OrderNumber>> TryGetExistentOrderNumbers();
        TryAsync<Unit> TrySaveOrder(ValidatedOrders order);
        TryAsync<Unit> TryRemoveOrder(ValidatedReturnOrders order);
    }
}