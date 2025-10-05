using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class OrderRepository(CustomerRepository customerRepository, ProductRepository productRepository,
        PaymentRepository paymentRepository) : RepositoryBase
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
            conn.Open();

            await using var countCommand = new SqliteCommand(countQuery, conn);

            var countParameters = new Dictionary<string, object>();

            foreach (var param in parameters)
            {
                countParameters.Add(param.name, param.value);
            }

            var totalCount = await ExecuteScalar(countQuery, countParameters);

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
                order.BillingAddress = await customerRepository.GetAddress(billingAddressId);
                order.ShippingAddress = await customerRepository.GetAddress(shippingAddressId);
                order.Payment = await paymentRepository.GetPayment(paymentId);
                order.OrderItems = await GetOrderItemsByOrderId(orderId);
                order.TrackingUpdates = await GetTrackingUpdatesByOrderId(orderId);
            }

            return order;
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

        public async Task<Order> CreateOrder(Order order)
        {
            var query =
                "INSERT INTO [Order] (OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt) " +
                "VALUES (@orderNumber, @customerId, @billingAddressId, @shippingAddressId, @totalPrice, @vatRate, @status, @paymentId, @deliveryMethod, " +
                "@estimatedDelivery, @contactPhoneNumber, @createdAt, @updatedAt); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@orderNumber", order.OrderNumber },
                { "@customerId", order.Customer?.Id ?? 0 },
                { "@billingAddressId", order.BillingAddress?.Id ?? 0 },
                { "@shippingAddressId", order.ShippingAddress?.Id ?? 0 },
                { "@totalPrice", order.TotalPrice },
                { "@vatRate", order.VATRate },
                { "@status", order.Status },
                { "@paymentId", order.Payment?.Id ?? 0 },
                { "@deliveryMethod", order.DeliveryMethod },
                { "@estimatedDelivery", (object?)order.EstimatedDelivery ?? DBNull.Value },
                { "@contactPhoneNumber", order.ContactPhoneNumber },
                { "@createdAt", order.CreatedAt },
                { "@updatedAt", order.UpdatedAt },
            };

            order.Id = await ExecuteScalar(query, parameters);
            return order;
        }

        public async Task UpdateOrderNumber(int orderId, string orderNumber)
        {
            var query =
                "UPDATE [Order] " +
                "SET OrderNumber = @orderNumber " +
                "WHERE Id = @orderId";

            var parameters = new Dictionary<string, object>
            {
                { "@orderId", orderId },
                { "@orderNumber", orderNumber }
            };

            await ExecuteNonQuery(query, parameters);
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

            var parameters = new Dictionary<string, object>
            {
                { "@orderId", orderId },
                { "@status", status },
                { "@deliveryMethod", (object?)deliveryMethod ?? DBNull.Value },
                { "@estimatedDelivery", (object?)estimatedDelivery ?? DBNull.Value },
                { "@updatedAt", DateTime.UtcNow },
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task<OrderItem> CreateOrderItem(OrderItem orderItem)
        {
            var query =
                "INSERT INTO OrderItem (OrderId, ProductId, Quantity, UnitPrice, TotalPrice, VATRate, CreatedAt) " +
                "VALUES (@orderId, @productId, @quantity, @unitPrice, @totalPrice, @vatRate, @createdAt); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@orderId", orderItem.OrderId },
                { "@productId", orderItem.Product?.Id ?? 0 },
                { "@quantity", orderItem.Quantity },
                { "@unitPrice", orderItem.UnitPrice },
                { "@totalPrice", orderItem.TotalPrice },
                { "@vatRate", orderItem.VATRate },
                { "@createdAt", orderItem.CreatedAt },
            };

            orderItem.Id = await ExecuteScalar(query, parameters);
            return orderItem;
        }

        public async Task<List<OrderItem>> CreateOrderItems(List<OrderItem> orderItems)
        {
            var createdItems = new List<OrderItem>();

            foreach (var orderItem in orderItems)
            {
                var created = await CreateOrderItem(orderItem);
                createdItems.Add(created);
            }

            return createdItems;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            var query =
                "SELECT Id, OrderId, ProductId, Quantity, UnitPrice, TotalPrice, VATRate, CreatedAt " +
                "FROM OrderItem " +
                "WHERE OrderId = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@orderId", orderId);

            var reader = await command.ExecuteReaderAsync();

            var orderItems = new List<OrderItem>();

            while (reader.Read())
            {
                orderItems.Add(new OrderItem
                {
                    Id = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    Product = new Product { Id = reader.GetInt32(2) },
                    Quantity = reader.GetInt32(3),
                    UnitPrice = reader.GetDouble(4),
                    TotalPrice = reader.GetDouble(5),
                    VATRate = reader.GetDouble(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }

            reader.Close();

            foreach (var orderItem in orderItems)
            {
                var product = await productRepository.GetProduct(orderItem.Product.Id);
                orderItem.Product = product;
            }

            return orderItems;
        }

        public async Task<OrderTrackingUpdate> CreateTrackingUpdate(OrderTrackingUpdate trackingUpdate)
        {
            var query =
                "INSERT INTO OrderTrackingUpdate (OrderId, UpdatedBy, Status, Note, CreatedAt) " +
                "VALUES (@orderId, @updatedBy, @status, @note, @createdAt); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@orderId", trackingUpdate.OrderId },
                { "@updatedBy", trackingUpdate.UpdatedBy },
                { "@status", trackingUpdate.Status },
                { "@note", trackingUpdate.Note },
                { "@createdAt", trackingUpdate.CreatedAt },
            };

            trackingUpdate.Id = await ExecuteScalar(query, parameters);
            return trackingUpdate;
        }

        public async Task<List<OrderTrackingUpdate>> GetTrackingUpdatesByOrderId(int orderId)
        {
            var trackingUpdates = new List<OrderTrackingUpdate>();

            var query =
                "SELECT Id, OrderId, UpdatedBy, Status, Note, CreatedAt " +
                "FROM OrderTrackingUpdate " +
                "WHERE OrderId = @orderId " +
                "ORDER BY CreatedAt ASC";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@orderId", orderId);

            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                trackingUpdates.Add(new OrderTrackingUpdate
                {
                    Id = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    UpdatedBy = reader.GetString(2),
                    Status = reader.GetString(3),
                    Note = reader.GetString(4),
                    CreatedAt = reader.GetDateTime(5)
                });
            }

            return trackingUpdates;
        }
    }
}