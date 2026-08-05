using Microsoft.Data.Sqlite;
using ScanProMovil.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;

namespace ScanProMovil.Services.Compras
{
    public class ComprasService : IComprasService
    {

        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "scanpro.db");
        private readonly string connectionString;
        public IHttpClientFactory httpFactory { get; set; }
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        public ComprasService(IHttpClientFactory HttpFactory)
        {
            connectionString = $"Data Source={dbPath}";
            this.httpFactory = HttpFactory;
            //CreateTableOrdersLocalSqlite();
            //FillDataExampleLocalSqllite();
        }

        public async Task<bool> SaveOrdersLocalSqliteAsync(OrdenCompra order)
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
                        command1.CommandText = @"INSERT INTO HeaderCompras (OrderNumber,OrderDate) 
                                                    VALUES ($order,$date)";

                        command1.Parameters.AddWithValue("$order", order.Numero);
                        command1.Parameters.AddWithValue("$date", order.Fecha);
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
                        command.CommandText = "SELECT * FROM HeaderCompras;";
                        using (var reader = command.ExecuteReader())
                        {
                            // Ejemplo: mostrar todas las columnas.
                            Debug.WriteLine("Header de la tabla Orden Compra:");
                            Debug.WriteLine("================================");
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
                        command2.CommandText = @"INSERT INTO DetalleCompras (OrderId, OrderNumber,ProductId,Cantidad) 
                                                VALUES ($orderId,$orderNumber,$productid,$cantidad)";

                        foreach (var item in order.Items)
                        {
                            command2.Parameters.Clear();
                            command2.Parameters.AddWithValue("$orderId", autoid);
                            command2.Parameters.AddWithValue("$orderNumber", order.Numero);
                            command2.Parameters.AddWithValue("$productid", item.Product_id);
                            command2.Parameters.AddWithValue("$cantidad", item.Cantidad);
                            await command2.ExecuteNonQueryAsync();
                        }
                        //ver el detalle de los items por consola.
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT * FROM DetalleCompras;";
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

        public void FillDataExampleLocalSqllite()
        {
            try
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();

                var comandoHeader = conn.CreateCommand();
                comandoHeader.CommandText = @"
                INSERT INTO HeaderCompras (OrderNumber, OrderDate)
                VALUES (@OrderNumber, @OrderDate);
                SELECT last_insert_rowid();";

                var comandoDetalle = conn.CreateCommand();
                comandoDetalle.CommandText = @"INSERT INTO DetalleCompras (OrderId, OrderNumber, 
                ProductId, Cantidad)
                VALUES (@OrderId, @OrderNumber, @ProductId, @Cantidad);";

                        // Datos de ejemplo para las 3 filas
                        var orders = new[]
                        {
                            new { OrderNumber = "ORD-001", ProductId = "PROD-100", Cantidad = 10.0 },
                            new { OrderNumber = "ORD-002", ProductId = "PROD-200", Cantidad = 5.5 },
                            new { OrderNumber = "ORD-003", ProductId = "PROD-300", Cantidad = 3.0 },
                        };

                foreach (var order in orders)
                {
                    // 1) Insertar en HeaderCompras y obtener el OrderId autogenerado
                    comandoHeader.Parameters.Clear();
                    comandoHeader.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                    comandoHeader.Parameters.AddWithValue("@OrderDate", DateTime.Now);

                    long newOrderId = (long)comandoHeader.ExecuteScalar()!;

                    // 2) Insertar el detalle asociado a ese OrderId
                    comandoDetalle.Parameters.Clear();
                    comandoDetalle.Parameters.AddWithValue("@OrderId", newOrderId);
                    comandoDetalle.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                    comandoDetalle.Parameters.AddWithValue("@ProductId", order.ProductId);
                    comandoDetalle.Parameters.AddWithValue("@Cantidad", order.Cantidad);

                    comandoDetalle.ExecuteNonQuery();
                }

                Debug.WriteLine("Se insertaron las 3 órdenes correctamente...");
            }
            catch (SqliteException ex)
            {
                Debug.Write("error al incluir data de ejemplo en las tablas de compras, " +
                    "error code =>" + ex.Message);
                
            }
        }

        public void CreateTableOrdersLocalSqlite()
        {
            try
            {
                var connection = new SqliteConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"CREATE TABLE HeaderCompras(" +
                                       "OrderId INTEGER PRIMARY KEY AUTOINCREMENT," +
                                       "OrderNumber TEXT NOT NULL," +
                                       "OrderDate DATETIME);" +
                                       "CREATE TABLE DetalleCompras(" +
                                       "DetailId INTEGER PRIMARY KEY AUTOINCREMENT," +
                                       "OrderId INTEGER NOT NULL," +
                                       "OrderNumber TEXT NOT NULL," +
                                       "ProductId TEXT NOT NULL," +
                                       "Cantidad DOUBLE," +
                                       "FOREIGN KEY (OrderId) REFERENCES HeaderCompras (OrderId) ON DELETE CASCADE" +
                                       ")";

                command.ExecuteNonQuery();
                Debug.Write("se crearon las tablas de sqlite correcamente...");
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("Error al tratar de crear las tablas de sqlite en el dispositivo, " +
                    "code error => " + ex.Message);
             
            }
        }
        public async Task<List<OrdenCompra>> GetOrdersLocalSqliteAsync()
        {
            var ordenes = new Dictionary<int, OrdenCompra>();

            try
            {
                //crea la cobnexion a sqlite db local del zebra.
                var connection = new SqliteConnection(connectionString);

                await connection.OpenAsync();
                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = @"SELECT a.orderId, a.orderNumber, a.orderDate, b.productId, b.cantidad " +
                                           "FROM HeaderCompras a LEFT JOIN DetalleCompras b ON a.orderId = b.orderId " +
                                           "Order By a.OrderNumber DESC";

                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var IndexCol = reader.GetOrdinal("orderId");

                            var orderId = reader.GetInt32(IndexCol);

                            if (!ordenes.TryGetValue(orderId, out OrdenCompra? order))
                            {
                                //guardar la orden
                                order = new OrdenCompra
                                {
                                    OrderId = orderId,
                                    Numero = reader.GetString(1),
                                    Fecha = reader.GetDateTime(2)
                                };

                                ordenes.Add(orderId, order);
                            }
                            //Guardar los items de productos
                            var IndexColProductId = reader.GetOrdinal("productId");


                            if (!reader.IsDBNull(IndexColProductId))
                            {
                                var ProductIdValue = reader.GetString(IndexColProductId);
                                var item = new DetalleCompra
                                {
                                    Product_id = ProductIdValue,
                                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                    OrderId = orderId,
                                    Numero = order.Numero
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
                    "'GetOrdersLocalSqliteAsync']" + " => " + ex.Message);
                throw;
            }

            return ordenes.Values.ToList();
        }

        public bool DeactivateOrder(string Orderid)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(string OrderId)
        {
            throw new NotImplementedException();
        }

        public OrdenCompra getOrderById(string OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrdenCompra>> getOrders()
        {
            throw new NotImplementedException();
        }

        public OrdenCompra UpdateOrder(OrdenCompra order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendPurchaseOrder(OrdenCompra order, CancellationToken cancellationToken = default)
        {
            // NUEVO: Validación del parámetro
            ArgumentNullException.ThrowIfNull(order);

            try
            {
                var url = $"api/ordencompra/addorder";
                var clienteHttp = httpFactory.CreateClient("scanpro");
                var responseServer = await clienteHttp.PostAsJsonAsync(url,order,
                    jsonOptions,cancellationToken);
                // Igual que antes
                if (responseServer.IsSuccessStatusCode)
                {
                    return true;
                }
              
                var error = await responseServer.Content.ReadAsStringAsync(cancellationToken);
                Debug.WriteLine($"HTTP {(int)responseServer.StatusCode}: {error}");
                return false;
              
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error HTTP: {ex.Message}");
                return false;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("La petición excedió el tiempo de espera.");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

       
       
    }
}
