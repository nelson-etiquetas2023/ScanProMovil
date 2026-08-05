
using ScanProMovil.Models;

namespace ScanProMovil.Services.Compras
{
    public interface IComprasService
    {
        Task<bool> SaveOrdersLocalSqliteAsync(OrdenCompra order);
        void CreateTableOrdersLocalSqlite();
        void FillDataExampleLocalSqllite();
        Task<List<OrdenCompra>> GetOrdersLocalSqliteAsync();
        Task<List<OrdenCompra>> getOrders();
        OrdenCompra getOrderById(string OrderId);
        Task<bool> SendPurchaseOrder(OrdenCompra order, CancellationToken cancellationToken = default);
        OrdenCompra UpdateOrder(OrdenCompra order);
        bool DeleteOrder(string OrderId);
        bool DeactivateOrder(string Orderid);
    }
}
