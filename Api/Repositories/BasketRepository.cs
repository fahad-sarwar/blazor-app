using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class BasketRepository(CustomerRepository customerRepository, ProductRepository productRepository) : RepositoryBase
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
                $"WHERE {fieldName} = @fieldValue";

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

                    basket.Items = await GetBasketItems(basket.Id);
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

            var parameters = new Dictionary<string, object>
            {
                { "@customerId", (object?)basket.Customer?.Id ?? DBNull.Value },
                { "@anonymousId", (object?)basket.AnonymousId ?? DBNull.Value },
                { "@createdAt", basket.CreatedAt }
            };

            basket.Id = await ExecuteScalar(query, parameters);
            return basket;
        }

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

            var parameters = new Dictionary<string, object>
            {
                { "@basketId", basketId }
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task RemoveAllBasketItems(int basketId)
        {
            var query = 
                "DELETE FROM BasketItem " +
                "WHERE BasketId = @basketId";

            var parameters = new Dictionary<string, object>
            {
                { "@basketId", basketId }
            };

            await ExecuteNonQuery(query, parameters);
        }

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

            var parameters = new Dictionary<string, object>
            {
                { "@basketId", basketItem.BasketId },
                { "@productId", basketItem.Product?.Id ?? 0 },
                { "@quantity", basketItem.Quantity },
                { "@price", basketItem.Price },
                { "@vatRate", basketItem.VATRate },
                { "@createdAt", basketItem.CreatedAt },
            };

            basketItem.Id = await ExecuteScalar(query, parameters);
            return basketItem;
        }

        public async Task UpdateBasketItemQuantity(int basketItemId, int quantity)
        {
            var query =
                "UPDATE BasketItem " +
                "SET Quantity = @quantity " +
                "WHERE Id = @basketItemId";

            var parameters = new Dictionary<string, object>
            {
                { "@basketItemId", basketItemId },
                { "@quantity", quantity }
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task DeleteBasketItem(int basketItemId)
        {
            var query =
                "DELETE FROM BasketItem " +
                "WHERE Id = @basketItemId";

            var parameters = new Dictionary<string, object>
            {
                { "@basketItemId", basketItemId }
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task<bool> BasketItemExists(int basketItemId)
        {
            var query =
                "SELECT COUNT(*) " +
                "FROM BasketItem " +
                "WHERE Id = @basketItemId";

            var parameters = new Dictionary<string, object>
            {
                { "@basketItemId", basketItemId }
            };

            var count = await ExecuteScalar(query, parameters);
            return count > 0;
        }
    }
}
