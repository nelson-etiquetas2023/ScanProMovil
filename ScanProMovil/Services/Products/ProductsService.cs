using Microsoft.Data.Sqlite;
using ScanProMovil.Models;
using SQLite;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ScanProMovil.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly SQLiteAsyncConnection database;

        IHttpClientFactory httpClient { get; set; }
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "scanpro.db");
        private readonly string connectionString;

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() 
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public ProductsService(IHttpClientFactory httpClient)
        {
            connectionString = $"Data Source={dbPath}";
            this.httpClient = httpClient;
            database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task<List<Product>> SearchProductsLocal(string searchText)
        {


            if (searchText == "") return await GetProductsLocal();
            
            return await database.Table<Product>()
                .Where(p => p.Product_Name.ToLower().Contains(searchText.ToLower()) 
                || p.product_code.Contains(searchText) || p.CodeBar.Contains(searchText))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsLocal() 
        {
            //obtener los productos de la base de datos local 
            List<Product> productsListLocal = new List<Product>();
            await using var conn = new SqliteConnection(connectionString);
            await conn.OpenAsync();
            using (var datalocal = conn.CreateCommand())
            {
                datalocal.CommandText = "SELECT * FROM productos;";
                using (var reader = await datalocal.ExecuteReaderAsync())
                {
                    //// Ejemplo: mostrar todas las columnas.
                    //Debug.WriteLine("...");
                    //Debug.WriteLine("data local Productos:       ");
                    //Debug.WriteLine("============================");
                    int rows = 0;
                    while (await reader.ReadAsync())
                    {
                        //Debug.WriteLine(
                        //    $"Product Code : {reader["product_code"]}, " +
                        //    $"Product Name: {reader["product_name"]}, " +
                        //    $"Costo: {reader["costo"]} " +
                        //    $"Codigo de Barra: {reader["codebar"]} "
                        //);
                        rows += 1;
                        Product producto = new Product
                        {
                            product_code = reader["product_code"].ToString()!,
                            Product_Name = reader["product_name"].ToString()!,
                            Costo = Convert.ToDecimal(reader["costo"])!,
                            CodeBar = reader["codebar"].ToString()!
                        };
                        productsListLocal.Add(producto);
                    }
                    //Debug.WriteLine("Tota Filas:" + Convert.ToString(rows));
                    //Debug.WriteLine("=====================");
                    return productsListLocal;
                }
            }
        }
        private void FillDataProducts() 
        {
            try
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();
                var comando = conn.CreateCommand();
                comando.CommandText = @"delete from productos";
                comando.ExecuteNonQuery();
                comando.CommandText = "";
                comando.CommandText = @"INSERT INTO productos (product_code,product_name,costo,codebar) 
                VALUES (@code, @description, @costo, @codebar)";
                //datos de ejemplo de productos.
                var ProductsFake = new[]
                {
                    new { product_code = "col001", product_name = "arroz la dominicana", costo = 300, codebar="300000506" }, 
                    new { product_code = "col002", product_name = "habichuelas san juanera", costo = 120, codebar="300000508"},
                    new { product_code = "col003", product_name = "kola real 1lts.", costo = 80, codebar="300000507"}
                };
                foreach (var Product in ProductsFake) 
                {
                    //insertar los producto en la tabla local.
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@code", Product.product_code);
                    comando.Parameters.AddWithValue("@description", Product.product_name);
                    comando.Parameters.AddWithValue("@costo", Product.costo);
                    comando.Parameters.AddWithValue("@codebar", Product.codebar);
                    comando.ExecuteNonQuery();
                }
                Debug.WriteLine("Se guardaron los datos de ejemplo e la tabla productos...");

                //listar los productos por la consola.
                using (var console = conn.CreateCommand())
                {
                    console.CommandText = "SELECT * FROM productos;";
                    using (var reader = console.ExecuteReader())
                    {
                        // Ejemplo: mostrar todas las columnas.
                        Debug.WriteLine("...");
                        Debug.WriteLine("data local Productos:       ");
                        Debug.WriteLine("============================");
                        int rows = 0;
                        while (reader.Read())
                        {
                            Debug.WriteLine(
                                $"Product Code : {reader["product_code"]}, " +
                                $"Product Name: {reader["product_name"]}, " +
                                $"Costo: {reader["costo"]} " +
                                $"Codigo de Barra: {reader["codebar"]} "
                            );
                            rows += 1;
                        }
                        Debug.WriteLine("Tota Filas:" + Convert.ToString(rows));
                        Debug.WriteLine("=====================");
                    }
                }
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("error al listar los producto guardados en el dispositivos de manera local..., " +
                 "code error => " + ex.Message);
            }
        }
        private async Task LocalSaveProductsSqlite(List<Product> listaProductsSincro) 
        {
            try
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();
                var comando = conn.CreateCommand();
                comando.CommandText = @"delete from productos";
                comando.ExecuteNonQuery();
                comando.CommandText = "";
                Debug.WriteLine("...");
                Debug.WriteLine("Limpiando tabla productos...");
                comando.CommandText = @"INSERT INTO productos (product_code,product_name,costo,codebar) 
                VALUES (@code, @description, @costo, @codebar)";
                //insert en la tabla local de productos.
                foreach (var Product in listaProductsSincro)
                {
                    //insertar los producto en la tabla local.
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@code", Product.product_code);
                    comando.Parameters.AddWithValue("@description", Product.Product_Name);
                    comando.Parameters.AddWithValue("@costo", Product.Costo);
                    comando.Parameters.AddWithValue("@codebar", Product.CodeBar);
                    comando.ExecuteNonQuery();
                }
                Debug.WriteLine("Se guardaron los datos de ejemplo e la tabla productos...");

                //listar los productos por la consola.
                using (var console = conn.CreateCommand())
                {
                    console.CommandText = "SELECT * FROM productos;";
                    using (var reader = console.ExecuteReader())
                    {
                        // Ejemplo: mostrar todas las columnas.
                
                        Debug.WriteLine("data local Productos:       ");
                        Debug.WriteLine("============================");
                        int rows = 0;
                        while (reader.Read())
                        {
                            Debug.WriteLine(
                                $"Product Code : {reader["product_code"]}, " +
                                $"Product Name: {reader["product_name"]}, " +
                                $"Costo: {reader["costo"]} " +
                                $"Codigo de Barra: {reader["codebar"]} "
                            );
                            rows += 1;
                        }
                        Debug.WriteLine("Tota Filas:" + Convert.ToString(rows));
                        Debug.WriteLine("=====================");
                    }
                }
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("error al listar los producto guardados en el dispositivos de manera local..., " +
                 "code error => " + ex.Message);
            }
        }
        private async Task<bool> CreateTableLocalProducts() 
        {
            try
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"CREATE TABLE IF NOT EXISTS productos (
                product_code TEXT PRIMARY KEY,
                product_name TEXT NOT NULL,
                costo DOUBLE,
                codebar TEXT NOT NULL)";
                command.ExecuteNonQuery();
                Debug.Write("se crearon las tablas de productos locales de producto correcamente...");
                FillDataProducts();
                return true;
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("Error al tratar de crear las tablas de productos locales en el dispositivo, " +
                   "code error => " + ex.Message);
                return false;
            }
        }
        public async Task<bool> SaveLocalProducts(List<Product> products)
        {
            await CreateTableLocalProducts();
            await LocalSaveProductsSqlite(products);
            return true;
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
