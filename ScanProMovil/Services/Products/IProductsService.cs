using ScanProMovil.Models;

namespace ScanProMovil.Services.Products
{
    public interface IProductsService
    {
        public Task<List<Product>> GetProducts();
        public Task<List<Product>> SearchProductsLocal(string searchText);
        public Task<List<Product>> GetProductsLocal();
        public Product GetPorductById(string productid);
        public Product AddProducts(Product producto);
        public Task<bool> UpdateProducts(int productid, Product producto);
        public bool DeleteProducts(string productid);
        public Task<bool> SaveLocalProducts(List<Product> products);
    }
}
