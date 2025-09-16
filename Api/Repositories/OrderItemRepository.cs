using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> CreateOrderItem(OrderItem orderItem);
        Task<List<OrderItem>> CreateOrderItems(List<OrderItem> orderItems);
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
    }

    public class OrderItemRepository(ILogger<OrderItemRepository> logger, IProductRepository productRepository) : RepositoryBase, IOrderItemRepository
    {
        public async Task<OrderItem> CreateOrderItem(OrderItem orderItem)
        {
            var query = 
                "INSERT INTO OrderItem (OrderId, ProductId, Quantity, UnitPrice, TotalPrice, VATRate, CreatedAt) " +
                "VALUES (@orderId, @productId, @quantity, @unitPrice, @totalPrice, @vatRate, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", orderItem.OrderId);
                command.Parameters.AddWithValue("@productId", orderItem.Product?.Id ?? 0);
                command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                command.Parameters.AddWithValue("@unitPrice", orderItem.UnitPrice);
                command.Parameters.AddWithValue("@totalPrice", orderItem.TotalPrice);
                command.Parameters.AddWithValue("@vatRate", orderItem.VATRate);
                command.Parameters.AddWithValue("@createdAt", orderItem.CreatedAt);

                var orderItemId = await command.ExecuteScalarAsync();
                orderItem.Id = Convert.ToInt32(orderItemId);
                return orderItem;
            }
            finally
            {
                conn.Close();
            }
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
            try
            {
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
            finally
            {
                conn.Close();
            }
        }
    }
}