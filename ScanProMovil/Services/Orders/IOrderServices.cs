using ScanProMovil.Models;

namespace ScanProMovil.Services.Orders
{
    public interface IOrderServices
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderById(string orderid);
        Task<bool> SaveOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(string idorder);
        Task<bool> CreateTableOrders();
        Task<List<Product>> GetProducts();
    }
}
