using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IBasketItemRepository
    {
        Task<List<BasketItem>?> GetBasketItems(int basketId);
        Task<BasketItem> CreateBasketItem(BasketItem basketItem);
        Task UpdateBasketItemQuantity(int basketItemId, int quantity);
        Task DeleteBasketItem(int basketItemId);
        Task<bool> BasketItemExists(int basketItemId);
    }

    public class BasketItemRepository(ILogger<BasketItemRepository> logger, IProductRepository productRepository) : RepositoryBase, IBasketItemRepository
    {
        public async Task<List<BasketItem>?> GetBasketItems(int basketId)
        {
            var query =
                "SELECT Id, BasketId, ProductId, Quantity, Price, VATRate, CreatedAt " +
                "FROM BasketItem " +
                "WHERE BasketId = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketId", basketId);

                var reader = await command.ExecuteReaderAsync();

                var basketItems = new List<BasketItem>();

                while (reader.Read())
                {
                    basketItems.Add(new BasketItem
                    {
                        Id = reader.GetInt32(0),
                        BasketId = reader.GetInt32(1),
                        Product = new Product { Id = reader.GetInt32(2) },
                        Quantity = reader.GetInt32(3),
                        Price = reader.GetDouble(4),
                        VATRate = reader.GetDouble(5),
                        CreatedAt = reader.GetDateTime(6)
                    });
                }

                reader.Close();

                foreach (var basketItem in basketItems)
                {
                    var product = await productRepository.GetProduct(basketItem.Product.Id);
                    basketItem.Product = product;
                }

                return basketItems;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<BasketItem> CreateBasketItem(BasketItem basketItem)
        {
            var query = 
                "INSERT INTO BasketItem (BasketId, ProductId, Quantity, Price, VATRate, CreatedAt) " +
                "VALUES (@basketId, @productId, @quantity, @price, @vatRate, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketId", basketItem.BasketId);
                command.Parameters.AddWithValue("@productId", basketItem.Product?.Id ?? 0);
                command.Parameters.AddWithValue("@quantity", basketItem.Quantity);
                command.Parameters.AddWithValue("@price", basketItem.Price);
                command.Parameters.AddWithValue("@vatRate", basketItem.VATRate);
                command.Parameters.AddWithValue("@createdAt", basketItem.CreatedAt);

                var basketItemId = await command.ExecuteScalarAsync();
                basketItem.Id = Convert.ToInt32(basketItemId);
                return basketItem;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task UpdateBasketItemQuantity(int basketItemId, int quantity)
        {
            var query = 
                "UPDATE BasketItem " +
                "SET Quantity = @quantity " + 
                "WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketItemId", basketItemId);
                command.Parameters.AddWithValue("@quantity", quantity);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task DeleteBasketItem(int basketItemId)
        {
            var query = 
                "DELETE FROM BasketItem " +
                "WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketItemId", basketItemId);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> BasketItemExists(int basketItemId)
        {
            var query = 
                "SELECT COUNT(*) " +
                "FROM BasketItem " +
                "WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketItemId", basketItemId);

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}