using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.Data.Sqlite;
using ScanProMovil.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ScanProMovil.Services.Orders
{
    public class OrderService : IOrderServices
    {
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "scanpro.db");
        private readonly string connectionString;
        public IHttpClientFactory httpFactory { get; set; }
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public OrderService(IHttpClientFactory HttpFactory)
        {
            connectionString = $"Data Source={dbPath}";
            this.httpFactory = HttpFactory;
        }

        public async Task<List<Order>> GetOrdersLocalSqliteAsync()
        {
            var ordenes = new Dictionary<int, Order>();

            try
            {
                //crea la cobnexion a sqlite db local del zebra.
                var connection = new SqliteConnection(connectionString);

                await connection.OpenAsync();

                //hacer mantenimiento borrar las ordenes.
                //using (var cmd = connection.CreateCommand()) 
                //{
                //    cmd.CommandText = "DELETE FROM Invoice;";
                        

                //    cmd.ExecuteNonQuery();
                //}

                //Comando sql para extraer las ordenes y items de la bd sqlite.
                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = @"SELECT a.orderId, a.orderNumber, a.orderDate, b.productId, b.cantidad " +
                                           "FROM Invoice a LEFT JOIN InvoiceDetails b ON a.orderId = b.orderId " +
                                           "Order By a.OrderNumber DESC";

                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var IndexCol = reader.GetOrdinal("orderId");

                            var orderId = reader.GetInt32(IndexCol);

                            if (!ordenes.TryGetValue(orderId, out Order? order))
                            {
                                //guardar la orden
                                order = new Order
                                {
                                    OrderId = orderId,
                                    OrderNumber = reader.GetString(1),
                                    OrderDate = reader.GetDateTime(2)
                                };

                                ordenes.Add(orderId, order);
                            }
                            //Guardar los items de productos
                            var IndexColProductId = reader.GetOrdinal("productId");


                            if (!reader.IsDBNull(IndexColProductId))
                            {
                                var ProductIdValue = reader.GetString(IndexColProductId);
                                var item = new OrderDetails
                                {
                                    productId = ProductIdValue,
                                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                    OrderId = orderId,
                                    OrderNumber = order.OrderNumber
                                };
                                order.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("Error en sqlite, service [OrderService-Metodo:" +
                    "'GetOrdersLocalSqliteAsync']" + " => " + ex.Message );
                throw;
            }

            return ordenes.Values.ToList();
        }

        public async Task<List<Product>> GetProductsRemoteApi()
        {
            var url = $"api/products/getproducts";
            var clientHttp = httpFactory.CreateClient("scanpro");
            var response = await clientHttp.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json)) return new List<Product>();
            var products = await JsonSerializer.DeserializeAsync<List<Product>>(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)), jsonOptions);
            return (products ?? new List<Product>());
        }

        public void CreateTableOrdersLocalSqlite()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"CREATE TABLE IF NOT EXISTS Invoice(" +
                                   "OrderId INTEGER PRIMARY KEY AUTOINCREMENT," +
                                   "OrderNumber TEXT NOT NULL," +
                                   "OrderDate DATETIME);" +
                                   "CREATE TABLE IF NOT EXISTS InvoiceDetails(" +
                                   "DetailId INTEGER PRIMARY KEY AUTOINCREMENT," +
                                   "OrderId INTEGER NOT NULL," +
                                   "OrderNumber TEXT NOT NULL," +
                                   "ProductId TEXT NOT NULL," +
                                   "Cantidad DOUBLE," +
                                   "FOREIGN KEY (OrderId) REFERENCES INVOICE (OrderId) ON DELETE CASCADE" +
                                   ")";

            command.ExecuteNonQuery();
        }

        public async Task<bool> DeleteOrderLocalSqliteAsync(string idorder)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString)) 
                {
                    await connection.OpenAsync();
                    using (var comando = connection.CreateCommand()) 
                    {
                        comando.CommandText = "DELETE FROM Invoice WHERE OrderNumber = @orderId";
                        comando.Parameters.AddWithValue("@orderId", idorder);

                        await comando.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("error al borrar orden: " + ex.Message);
                return false;
            }
        }

        public Task<Order> GetOrderById(string orderid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveOrdersLocalSqliteAsync(Order order)
        {
            try
            {
                //conexion principal para todos los comandos sqlite.
                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();
                    //Guardar el Encabezado de la Orden.
                    using (var command1 = connection.CreateCommand())
                    {
                        command1.CommandText = @"INSERT INTO Invoice (OrderNumber,OrderDate) 
                                                    VALUES ($order,$date)";

                        command1.Parameters.AddWithValue("$order", order.OrderNumber );
                        command1.Parameters.AddWithValue("$date", order.OrderDate );
                        await command1.ExecuteNonQueryAsync();
                    }
                    //devolver el id autoincrement de la bd. sqlite.
                    int autoid = 0;
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT last_insert_rowid()";
                        object? scalar = await cmd.ExecuteScalarAsync();

                        long id = 0;
                        if (scalar is long l) id = l;
                        else if (scalar is int i) id = i;
                        else if (scalar is DBNull || scalar == null) id = 0;
                        else id = Convert.ToInt64(scalar);
                        autoid = (int)id;
                    }
                    //listar las ordenes por la consola.
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Invoice;";
                        using (var reader = command.ExecuteReader())
                        {
                            // Ejemplo: mostrar todas las columnas.
                            Debug.WriteLine("Data Invoice:");
                            Debug.WriteLine("=============");
                            int rows = 0;
                            while (reader.Read())
                            {
                                Debug.WriteLine(
                                    $"Id: {reader["OrderId"]}, " +
                                    $"Number: {reader["OrderNumber"]}, " +
                                    $"Date: {reader["OrderDate"]}"
                                );
                                rows += 1;
                            }
                            Debug.WriteLine("Tota Filas:" + Convert.ToString(rows));
                            Debug.WriteLine("=====================");
                        }
                    }
                    //Guardar los items de las ordenes.
                    using (var command2 = connection.CreateCommand())
                    {
                        command2.CommandText = @"INSERT INTO InvoiceDetails (OrderId, OrderNumber,ProductId,Cantidad) 
                                                VALUES ($orderId,$orderNumber,$productid,$cantidad)";

                        foreach (var item in order.Items)
                        {
                            command2.Parameters.Clear();
                            command2.Parameters.AddWithValue("$orderId", autoid);
                            command2.Parameters.AddWithValue("$orderNumber", order.OrderNumber);
                            command2.Parameters.AddWithValue("$productid", item.productId);
                            command2.Parameters.AddWithValue("$cantidad", item.Cantidad);
                            await command2.ExecuteNonQueryAsync();
                        }
                        //ver el detalle de los items por consola.
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT * FROM InvoiceDetails;";
                            using (var reader = command.ExecuteReader())
                            {
                                // Ejemplo: mostrar todas las columnas.
                                Debug.WriteLine("Items:       ");
                                Debug.WriteLine("=============");
                                int rows = 0;
                                while (reader.Read())
                                {

                                    Debug.WriteLine(
                                        $"Id: {reader["OrderId"]}, " +
                                        $"Number: {reader["OrderNumber"]}, " +
                                        $"product id: {reader["productid"]}" +
                                        $"cantidad: {reader["cantidad"]}"
                                    );
                                    rows += 1;
                                }
                                Debug.WriteLine("Tota Filas:" + Convert.ToString(rows));
                                Debug.WriteLine("=====================");
                            }
                        }
                    }
                    return true;
                }
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("error sqlite:" + ex.Message);
                return false;
            }
        }

        public Task<bool> UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public List<Order> LoadDataFake()
        {
            var ordenes = new List<Order>();
            
            //ORDEN 0
            var orden0 = new Order();
            orden0.OrderNumber = "1000";
            orden0.OrderId = 1;
            orden0.OrderDate = DateTime.Today;
            orden0.ItemsNumber = 23;
            orden0.TotalCosto = 123.45;
            var producto1 = new OrderDetails { productId = "101", Cantidad = 10, OrderId = 1, OrderNumber = "1000" };
            var producto2 = new OrderDetails { productId = "102", Cantidad = 11, OrderId = 1, OrderNumber = "1000" };
            var producto3 = new OrderDetails { productId = "103", Cantidad = 12, OrderId = 1, OrderNumber = "1000" };
            orden0.Items.Add(producto1);
            orden0.Items.Add(producto2);
            orden0.Items.Add(producto3);

            //ORDEN 1
            var orden1 = new Order();
            orden1.OrderNumber = "1001";
            orden1.OrderId = 2;
            orden1.OrderDate = DateTime.Today;
            var producto11 = new OrderDetails { productId = "107", Cantidad = 10, OrderId = 1, OrderNumber = "1000" };
            var producto12 = new OrderDetails { productId = "112", Cantidad = 11, OrderId = 1, OrderNumber = "1000" };
            var producto13 = new OrderDetails { productId = "113", Cantidad = 12, OrderId = 1, OrderNumber = "1000" };
            orden1.Items.Add(producto11);
            orden1.Items.Add(producto12);
            orden1.Items.Add(producto13);

            var orden2 = new Order();
            orden2.OrderNumber = "1002";
            orden2.OrderId = 3;
            orden2.OrderDate = DateTime.Today.AddDays(30);
            orden2.Status = 1;

            var orden3 = new Order();
            orden3.OrderNumber = "1003";
            orden3.OrderId = 4;
            orden3.OrderDate = DateTime.Today.AddDays(15);
            orden3.Status = 2;

            var orden4 = new Order();
            orden4.OrderNumber = "1004";
            orden4.OrderId = 5;
            orden4.OrderDate = DateTime.Today.AddDays(18);

            var orden5 = new Order();
            orden5.OrderNumber = "1007";
            orden5.OrderId = 6;
            orden5.OrderDate = DateTime.Today.AddDays(7);
            orden5.ItemsNumber = 11;
            orden5.TotalCosto = 3245.60;
            orden5.Status = 2;

            var orden6 = new Order();
            orden6.OrderNumber = "1009";
            orden6.OrderId = 7;
            orden6.OrderDate = DateTime.Today.AddDays(27);
            orden6.ItemsNumber = 32;
            orden6.TotalCosto = 6352.15;
            orden6.Status = 1;

            ordenes.Add(orden0);
            ordenes.Add(orden1);
            ordenes.Add(orden2);
            ordenes.Add(orden3);
            ordenes.Add(orden4);
            ordenes.Add(orden5);
            ordenes.Add(orden6);
            return ordenes;
        }
    }
}
