using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketByAnonymousId(string anonymousId);
        Task<Basket?> GetBasketByCustomerId(int customerId);
        Task<Basket?> GetBasketById(int basketId);
        Task<Basket> CreateBasket(Basket basket);
        Task<Basket?> GetOrCreateBasket(string? anonymousId, int? customerId);
        Task DeleteBasket(int basketId);
        Task RemoveAllBasketItems(int basketId);
    }

    public class BasketRepository(
        ILogger<BasketRepository> logger,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IBasketItemRepository basketItemRepository) : RepositoryBase, IBasketRepository
    {
        public async Task<Basket?> GetBasketByAnonymousId(string anonymousId)
        {
            return await GetBasketByField("AnonymousId", anonymousId);
        }

        public async Task<Basket?> GetBasketByCustomerId(int customerId)
        {
            return await GetBasketByField("CustomerId", customerId);
        }

        private async Task<Basket?> GetBasketByField(string fieldName, object fieldValue)
        {
            Basket? basket = null;

            var query =
                "SELECT Id, CustomerId, AnonymousId, CreatedAt " +
                "FROM Basket " +
                "WHERE {fieldName} = @fieldValue";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@fieldValue", fieldValue);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var customerId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);

                    basket = new Basket
                    {
                        Id = reader.GetInt32(0),
                        AnonymousId = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CreatedAt = reader.GetDateTime(3),
                        Items = new List<BasketItem>()
                    };

                    reader.Close();

                    if (customerId.HasValue)
                    {
                        basket.Customer = await customerRepository.GetCustomerById(customerId.Value);
                    }

                    await basketItemRepository.GetBasketItems(basket.Id);
                }

                return basket;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Basket> CreateBasket(Basket basket)
        {
            var query =
                "INSERT INTO Basket (CustomerId, AnonymousId, CreatedAt) " +
                "VALUES (@customerId, @anonymousId, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@customerId", (object?)basket.Customer?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@anonymousId", (object?)basket.AnonymousId ?? DBNull.Value);
                command.Parameters.AddWithValue("@createdAt", basket.CreatedAt);

                var basketId = await command.ExecuteScalarAsync();
                basket.Id = Convert.ToInt32(basketId);
                return basket;
            }
            finally
            {
                conn.Close();
            }
        }

        // TODO Check if this is needed
        public async Task<Basket?> GetOrCreateBasket(string? anonymousId, int? customerId)
        {
            Basket? basket = null;

            if (!string.IsNullOrEmpty(anonymousId))
            {
                basket = await GetBasketByAnonymousId(anonymousId);
            }
            else if (customerId.HasValue)
            {
                basket = await GetBasketByCustomerId(customerId.Value);
            }

            if (basket == null && (!string.IsNullOrEmpty(anonymousId) || customerId.HasValue))
            {
                var newBasket = new Basket
                {
                    AnonymousId = anonymousId,
                    CreatedAt = DateTime.UtcNow
                };

                if (customerId.HasValue)
                {
                    newBasket.Customer = await customerRepository.GetCustomerById(customerId.Value);
                }

                basket = await CreateBasket(newBasket);
            }

            return basket;
        }
        public async Task<Basket?> GetBasketById(int basketId)
        {
            return await GetBasketByField("Id", basketId);
        }

        public async Task DeleteBasket(int basketId)
        {
            var query = 
                "DELETE FROM Basket " +
                "WHERE Id = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketId", basketId);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task RemoveAllBasketItems(int basketId)
        {
            var query = 
                "DELETE FROM BasketItem " +
                "WHERE BasketId = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@basketId", basketId);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
