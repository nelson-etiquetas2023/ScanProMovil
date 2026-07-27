
using ScanProMovil.Models;
using System.Security.Cryptography;

namespace ScanProMovil.Services.Compras
{
    public interface IComprasService
    {
        void CreateTableOrdersLocalSqlite();
        void FillDataExampleLocalSqllite();
        Task<List<Order>> GetOrdersLocalSqliteAsync();
        Task<List<Order>> getOrders();
        Order getOrderById(string OrderId);
        bool SincroOrder(string OrderId);
        Order UpdateOrder(Order order);
        bool DeleteOrder(string OrderId);
        bool DeactivateOrder(string Orderid);
    }
}
