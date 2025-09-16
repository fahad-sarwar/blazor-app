using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IWishlistRepository
    {
        Task<(List<Product> Products, int TotalCount)> GetWishlistProducts(int customerId, int page = 1, int pageSize = 10);
        Task<bool> IsProductInWishlist(int customerId, int productId);
        Task AddToWishlist(int customerId, int productId);
        Task RemoveFromWishlist(int customerId, int productId);
    }

    public class WishlistRepository(ILogger<WishlistRepository> logger, IProductRepository productRepository) : RepositoryBase, IWishlistRepository
    {
        public async Task<(List<Product> Products, int TotalCount)> GetWishlistProducts(int customerId, int page = 1, int pageSize = 10)
        {
            var products = new List<Product>();

            var countQuery = 
                "SELECT COUNT(*) " +
                "FROM Wishlist " +
                "WHERE CustomerId = @customerId";

            var wishlistQuery =
                "SELECT ProductId " +
                "FROM Wishlist " +
                "WHERE CustomerId = @customerId " +
                "ORDER BY CreatedAt DESC " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);
            try
            {
                conn.Open();

                await using var countCommand = new SqliteCommand(countQuery, conn);
                countCommand.Parameters.AddWithValue("@customerId", customerId);
                var totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());

                await using var wishlistCommand = new SqliteCommand(wishlistQuery, conn);
                wishlistCommand.Parameters.AddWithValue("@customerId", customerId);
                wishlistCommand.Parameters.AddWithValue("@pageSize", pageSize);
                wishlistCommand.Parameters.AddWithValue("@offset", (page - 1) * pageSize);

                var productIds = new List<int>();
                var reader = await wishlistCommand.ExecuteReaderAsync();

                while (reader.Read())
                {
                    productIds.Add(reader.GetInt32(0));
                }

                reader.Close();

                foreach (var productId in productIds)
                {
                    var product = await productRepository.GetProduct(productId);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }

                return (products, totalCount);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> IsProductInWishlist(int customerId, int productId)
        {
            var query = 
                "SELECT COUNT(*) " +
                "FROM Wishlist " +
                "WHERE CustomerId = @customerId AND ProductId = @productId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@productId", productId);

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task AddToWishlist(int customerId, int productId)
        {
            var query = 
                "INSERT INTO Wishlist (CustomerId, ProductId, CreatedAt) " +
                "VALUES (@customerId, @productId, @createdAt)";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task RemoveFromWishlist(int customerId, int productId)
        {
            var query =
                "DELETE FROM Wishlist " +
                "WHERE CustomerId = @customerId AND ProductId = @productId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@productId", productId);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}