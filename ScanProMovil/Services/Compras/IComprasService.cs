
using ScanProMovil.Models;

namespace ScanProMovil.Services.Compras
{
    public interface IComprasService
    {
        Task<List<Order>> getOrders();
        Order getOrderById(string OrderId);
        bool SincroOrder(string OrderId);
        Order UpdateOrder(Order order);
        bool DeleteOrder(string OrderId);
        bool DeactivateOrder(string Orderid);
    }
}
