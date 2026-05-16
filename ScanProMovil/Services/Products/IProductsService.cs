using ScanProMovil.Models;

namespace ScanProMovil.Services.Products
{
    public interface IProductsService
    {
        public Task<List<Product>> GetProducts();
        public Product GetPorductById(string productid);
        public Product AddProducts(Product producto);
        public Task<bool> UpdateProducts(int productid, Product producto);
        public bool DeleteProducts(string productid);
    }
}
