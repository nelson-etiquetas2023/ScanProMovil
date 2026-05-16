using ScanProMovil.Models;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;

namespace ScanProMovil.Services.Products
{
    public class ProductsService : IProductsService
    {
        IHttpClientFactory httpClient { get; set; }

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() 
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public ProductsService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;    
        }
        public async Task<List<Product>> GetProducts()
        {
            var url = $"api/products/getproducts";
            var clientHttp = httpClient.CreateClient("scanpro");
            var response = await clientHttp.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json)) return new List<Product>();
            var products = await JsonSerializer.DeserializeAsync<List<Product>>(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)),jsonOptions);
            return (products ?? new List<Product>());
        }
        public Product AddProducts(Product producto)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProducts(string productid)
        {
            throw new NotImplementedException();
        }

        public Product GetPorductById(string productid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProducts(int id, Product producto)
        {
            //utilizo una tupla para pasar 2 parametros a la api.
            var parametros = new ParametrosUpdateProducts(id, producto);
            var url = $"api/products/updateproducts";
            var json = JsonSerializer.Serialize(parametros, jsonOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            var clientHttp = httpClient.CreateClient("scanpro");
            var response = await clientHttp.PutAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
