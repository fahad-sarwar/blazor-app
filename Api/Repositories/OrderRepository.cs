using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class OrderRepository : RepositoryBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PaymentRepository _paymentRepository;

        public OrderRepository(CustomerRepository customerRepository, ProductRepository productRepository,
            PaymentRepository paymentRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<List<Order>> GetOrdersByCustomerId(int customerId, string? orderNumber = null)
        {
            var whereConditions = new List<string> { "CustomerId = @customerId" };
            var parameters = new DynamicParameters();
            parameters.Add("customerId", customerId);

            if (!string.IsNullOrEmpty(orderNumber))
            {
                whereConditions.Add("OrderNumber = @orderNumber");
                parameters.Add("orderNumber", orderNumber);
            }

            var whereClause = "WHERE " + string.Join(" AND ", whereConditions);

            var dataQuery =
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " +
                "FROM [Order] " +
                $"{whereClause} " +
                "ORDER BY CreatedAt DESC";

            await using var conn = new SqliteConnection(ConnectionString);

            var orderData = await conn.QueryAsync<dynamic>(dataQuery, parameters);

            var orders = orderData.Select(o => new Order
            {
                Id = Convert.ToInt32(o.Id),
                OrderNumber = o.OrderNumber,
                Customer = new Customer { Id = Convert.ToInt32(o.CustomerId) },
                BillingAddress = new Address { Id = Convert.ToInt32(o.BillingAddressId) },
                ShippingAddress = new Address { Id = Convert.ToInt32(o.ShippingAddressId) },
                TotalPrice = o.TotalPrice,
                VATRate = o.VATRate,
                Status = o.Status,
                Payment = new Payment { Id = Convert.ToInt32(o.PaymentId) },
                DeliveryMethod = o.DeliveryMethod,
                EstimatedDelivery = o.EstimatedDelivery != null ? DateTime.Parse(o.EstimatedDelivery.ToString()) : null,
                ContactPhoneNumber = o.ContactPhoneNumber,
                CreatedAt = DateTime.Parse(o.CreatedAt.ToString()),
                UpdatedAt = DateTime.Parse(o.UpdatedAt.ToString())
            }).ToList();

            foreach (var order in orders)
            {
                var customer = await _customerRepository.GetCustomerById(order.Customer.Id);
                order.Customer = customer;
            }

            return orders;
        }

        public async Task<Order?> GetOrder(int orderId, int customerId)
        {
            var query =
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " +
                "FROM [Order] " +
                "WHERE Id = @orderId AND CustomerId = @customerId";

            await using var conn = new SqliteConnection(ConnectionString);

            var orderData = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { orderId, customerId });

            if (orderData == null)
                return null;

            var order = new Order
            {
                Id = Convert.ToInt32(orderData.Id),
                OrderNumber = orderData.OrderNumber,
                TotalPrice = orderData.TotalPrice,
                VATRate = orderData.VATRate,
                Status = orderData.Status,
                DeliveryMethod = orderData.DeliveryMethod,
                EstimatedDelivery = orderData.EstimatedDelivery != null ? DateTime.Parse(orderData.EstimatedDelivery.ToString()) : null,
                ContactPhoneNumber = orderData.ContactPhoneNumber,
                CreatedAt = DateTime.Parse(orderData.CreatedAt.ToString()),
                UpdatedAt = DateTime.Parse(orderData.UpdatedAt.ToString())
            };

            order.Customer = await _customerRepository.GetCustomerById(customerId);
            order.BillingAddress = await _customerRepository.GetAddress(Convert.ToInt32(orderData.BillingAddressId));
            order.ShippingAddress = await _customerRepository.GetAddress(Convert.ToInt32(orderData.ShippingAddressId));
            order.Payment = await _paymentRepository.GetPayment(Convert.ToInt32(orderData.PaymentId));
            order.OrderItems = await GetOrderItemsByOrderId(orderId);
            order.TrackingUpdates = await GetTrackingUpdatesByOrderId(orderId);

            return order;
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            var query =
                "SELECT Id, OrderNumber, CustomerId, BillingAddressId, ShippingAddressId, TotalPrice, VATRate, Status, PaymentId, DeliveryMethod, " +
                "EstimatedDelivery, ContactPhoneNumber, CreatedAt, UpdatedAt " +
                "FROM [Order] " +
                "WHERE Id = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<Order>(query, new { orderId });
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

            var orderId = await conn.QuerySingleAsync<int>(query, new
            {
                orderNumber = order.OrderNumber,
                customerId = order.Customer?.Id ?? 0,
                billingAddressId = order.BillingAddress?.Id ?? 0,
                shippingAddressId = order.ShippingAddress?.Id ?? 0,
                totalPrice = order.TotalPrice,
                vatRate = order.VATRate,
                status = order.Status,
                paymentId = order.Payment?.Id ?? 0,
                deliveryMethod = order.DeliveryMethod,
                estimatedDelivery = order.EstimatedDelivery,
                contactPhoneNumber = order.ContactPhoneNumber,
                createdAt = order.CreatedAt,
                updatedAt = order.UpdatedAt
            });

            order.Id = orderId;
            return order;
        }

        public async Task UpdateOrderNumber(int orderId, string orderNumber)
        {
            var query = "UPDATE [Order] SET OrderNumber = @orderNumber WHERE Id = @orderId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { orderId, orderNumber });
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

            await conn.ExecuteAsync(query, new
            {
                orderId,
                status,
                deliveryMethod,
                estimatedDelivery,
                updatedAt = DateTime.UtcNow
            });
        }

        public async Task<OrderItem> CreateOrderItem(OrderItem orderItem)
        {
            var query =
                "INSERT INTO OrderItem (OrderId, ProductId, Quantity, UnitPrice, TotalPrice, VATRate, CreatedAt) " +
                "VALUES (@orderId, @productId, @quantity, @unitPrice, @totalPrice, @vatRate, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var orderItemId = await conn.QuerySingleAsync<int>(query, new
            {
                orderId = orderItem.OrderId,
                productId = orderItem.Product?.Id ?? 0,
                quantity = orderItem.Quantity,
                unitPrice = orderItem.UnitPrice,
                totalPrice = orderItem.TotalPrice,
                vatRate = orderItem.VATRate,
                createdAt = orderItem.CreatedAt
            });

            orderItem.Id = orderItemId;
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

            var orderItemData = await conn.QueryAsync<dynamic>(query, new { orderId });

            var orderItems = orderItemData.Select(oi => new OrderItem
            {
                Id = Convert.ToInt32(oi.Id),
                OrderId = Convert.ToInt32(oi.OrderId),
                Product = new Product { Id = Convert.ToInt32(oi.ProductId) },
                Quantity = Convert.ToInt32(oi.Quantity),
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice,
                VATRate = oi.VATRate,
                CreatedAt = DateTime.Parse(oi.CreatedAt.ToString())
            }).ToList();

            foreach (var orderItem in orderItems)
            {
                var product = await _productRepository.GetProduct(orderItem.Product.Id);
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

            await using var conn = new SqliteConnection(ConnectionString);

            var orderTrackingUpdateId = await conn.QuerySingleAsync<int>(query, new
            {
                orderId = trackingUpdate.OrderId,
                updatedBy = trackingUpdate.UpdatedBy,
                status = trackingUpdate.Status,
                note = trackingUpdate.Note,
                createdAt = trackingUpdate.CreatedAt
            });

            trackingUpdate.Id = orderTrackingUpdateId;
            return trackingUpdate;
        }

        public async Task<List<OrderTrackingUpdate>> GetTrackingUpdatesByOrderId(int orderId)
        {
            var query =
                "SELECT Id, OrderId, UpdatedBy, Status, Note, CreatedAt " +
                "FROM OrderTrackingUpdate " +
                "WHERE OrderId = @orderId " +
                "ORDER BY CreatedAt ASC";

            await using var conn = new SqliteConnection(ConnectionString);

            var trackingUpdates = await conn.QueryAsync<OrderTrackingUpdate>(query, new { orderId });

            return trackingUpdates.ToList();
        }
    }
}