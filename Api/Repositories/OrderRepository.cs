using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class OrderRepository(
        CustomerRepository customerRepository,
        AddressRepository addressRepository,
        PaymentRepository paymentRepository,
        OrderItemRepository orderItemRepository,
        OrderTrackingUpdateRepository trackingUpdateRepository) : RepositoryBase
    {
        public async Task<(List<Order> Orders, int TotalCount)> GetOrdersByCustomerId(int customerId, string? orderNumber = null, int page = 1, int pageSize = 10)
        {
            var whereConditions = new List<string> { "CustomerId = @customerId" };
            var parameters = new List<(string name, object value)> { ("@customerId", customerId) };

            if (!string.IsNullOrEmpty(orderNumber))
            {
                whereConditions.Add("OrderNumber = @orderNumber");
                parameters.Add(("@orderNumber", orderNumber));
            }

            var whereClause = "WHERE " + string.Join(" AND ", whereConditions);

            var countQuery = 
                "SELECT COUNT(*) " +
                "FROM [Order] " +
                $"{whereClause}";

            var dataQuery = 
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " + 
                "FROM [Order] " +
                $"{whereClause} " +
                "ORDER BY CreatedAt DESC " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);
            try
            {
                conn.Open();

                await using var countCommand = new SqliteCommand(countQuery, conn);
                foreach (var param in parameters)
                {
                    countCommand.Parameters.AddWithValue(param.name, param.value);
                }
                var totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());

                await using var dataCommand = new SqliteCommand(dataQuery, conn);

                parameters.Add(("@pageSize", pageSize));
                parameters.Add(("@offset", (page - 1) * pageSize));

                foreach (var param in parameters)
                {
                    dataCommand.Parameters.AddWithValue(param.name, param.value);
                }

                var reader = await dataCommand.ExecuteReaderAsync();

                var orders = new List<Order>();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        Id = reader.GetInt32(0),
                        OrderNumber = reader.GetString(1),
                        Customer = new Customer { Id = reader.GetInt32(2) },
                        BillingAddress = new Address { Id = reader.GetInt32(3) },
                        ShippingAddress = new Address { Id = reader.GetInt32(4) },
                        TotalPrice = reader.GetDouble(5),
                        VATRate = reader.GetDouble(6),
                        Status = reader.GetString(7),
                        Payment = new Payment { Id = reader.GetInt32(8) },
                        DeliveryMethod = reader.GetString(9),
                        EstimatedDelivery = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                        ContactPhoneNumber = reader.GetString(11),
                        CreatedAt = reader.GetDateTime(12),
                        UpdatedAt = reader.GetDateTime(13)
                    });
                }

                reader.Close();

                foreach (var order in orders)
                {
                    var customer = await customerRepository.GetCustomerById(order.Customer.Id);
                    order.Customer = customer;
                }

                return (orders, totalCount);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Order?> GetOrder(int orderId, int customerId)
        {
            Order? order = null;

            var query = 
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " +
                "FROM [Order] " +
                "WHERE Id = @orderId AND CustomerId = @customerId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@customerId", customerId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var billingAddressId = reader.GetInt32(3);
                    var shippingAddressId = reader.GetInt32(4);
                    var paymentId = reader.GetInt32(8);

                    order = new Order
                    {
                        Id = reader.GetInt32(0),
                        OrderNumber = reader.GetString(1),
                        TotalPrice = reader.GetDouble(5),
                        VATRate = reader.GetDouble(6),
                        Status = reader.GetString(7),
                        DeliveryMethod = reader.GetString(9),
                        EstimatedDelivery = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                        ContactPhoneNumber = reader.GetString(11),
                        CreatedAt = reader.GetDateTime(12),
                        UpdatedAt = reader.GetDateTime(13)
                    };

                    reader.Close();

                    order.Customer = await customerRepository.GetCustomerById(customerId);
                    order.BillingAddress = await addressRepository.GetAddress(billingAddressId);
                    order.ShippingAddress = await addressRepository.GetAddress(shippingAddressId);
                    order.Payment = await paymentRepository.GetPayment(paymentId);
                    order.OrderItems = await orderItemRepository.GetOrderItemsByOrderId(orderId);
                    order.TrackingUpdates = await trackingUpdateRepository.GetTrackingUpdatesByOrderId(orderId);
                }

                return order;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            Order? order = null;

            var query = 
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " +
                "FROM [Order] " +
                "WHERE Id = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);

            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", orderId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    order = new Order
                    {
                        Id = reader.GetInt32(0),
                        OrderNumber = reader.GetString(1),
                        TotalPrice = reader.GetDouble(5),
                        VATRate = reader.GetDouble(6),
                        Status = reader.GetString(7),
                        DeliveryMethod = reader.GetString(9),
                        EstimatedDelivery = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                        ContactPhoneNumber = reader.GetString(11),
                        CreatedAt = reader.GetDateTime(12),
                        UpdatedAt = reader.GetDateTime(13)
                    };
                }

                return order;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var query = 
                "INSERT INTO [Order] (OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt) " +
                "VALUES (@orderNumber, @customerId, @billingAddressId, @shippingAddressId, @totalPrice, @vatRate, @status, @paymentId, @deliveryMethod, " +
                "@estimatedDelivery, @contactPhoneNumber, @createdAt, @updatedAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderNumber", order.OrderNumber);
                command.Parameters.AddWithValue("@customerId", order.Customer?.Id ?? 0);
                command.Parameters.AddWithValue("@billingAddressId", order.BillingAddress?.Id ?? 0);
                command.Parameters.AddWithValue("@shippingAddressId", order.ShippingAddress?.Id ?? 0);
                command.Parameters.AddWithValue("@totalPrice", order.TotalPrice);
                command.Parameters.AddWithValue("@vatRate", order.VATRate);
                command.Parameters.AddWithValue("@status", order.Status);
                command.Parameters.AddWithValue("@paymentId", order.Payment?.Id ?? 0);
                command.Parameters.AddWithValue("@deliveryMethod", order.DeliveryMethod);
                command.Parameters.AddWithValue("@estimatedDelivery", (object?)order.EstimatedDelivery ?? DBNull.Value);
                command.Parameters.AddWithValue("@contactPhoneNumber", order.ContactPhoneNumber);
                command.Parameters.AddWithValue("@createdAt", order.CreatedAt);
                command.Parameters.AddWithValue("@updatedAt", order.UpdatedAt);

                var orderId = await command.ExecuteScalarAsync();
                order.Id = Convert.ToInt32(orderId);
                return order;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task UpdateOrderNumber(int orderId, string orderNumber)
        {
            var query = 
                "UPDATE [Order] " +
                "SET OrderNumber = @orderNumber " +
                "WHERE Id = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task UpdateOrderStatus(int orderId, string status, string? deliveryMethod = null, DateTime? estimatedDelivery = null)
        {
            var query = @"
                UPDATE [Order] 
                SET Status = @status, 
                    DeliveryMethod = COALESCE(@deliveryMethod, DeliveryMethod),
                    EstimatedDelivery = COALESCE(@estimatedDelivery, EstimatedDelivery),
                    UpdatedAt = @updatedAt
                WHERE Id = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@deliveryMethod", (object?)deliveryMethod ?? DBNull.Value);
            command.Parameters.AddWithValue("@estimatedDelivery", (object?)estimatedDelivery ?? DBNull.Value);
            command.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);

            try
            {
                conn.Open();
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}