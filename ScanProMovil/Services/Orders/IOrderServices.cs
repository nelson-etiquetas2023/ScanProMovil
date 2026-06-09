using ScanProMovil.Models;

namespace ScanProMovil.Services.Orders
{
    public interface IOrderServices
    {
        Task<List<Order>> GetOrdersLocalSqliteAsync();
        Task<List<Product>> GetProductsRemoteApi();
        Task<Order> GetOrderById(string orderid);
        Task<bool> SaveOrdersLocalSqliteAsync(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(string idorder);
        void CreateTableOrdersLocalSqlite();
        List<Order>LoadDataFake();
    }
}