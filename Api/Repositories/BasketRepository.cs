using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class BasketRepository : RepositoryBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;

        public BasketRepository(CustomerRepository customerRepository, ProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

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
            var query =
                "SELECT Id, CustomerId, AnonymousId, CreatedAt " +
                "FROM Basket " +
                $"WHERE {fieldName} = @fieldValue";

            await using var conn = new SqliteConnection(ConnectionString);

            var basket = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { fieldValue });

            if (basket == null)
                return null;

            var result = new Basket
            {
                Id = Convert.ToInt32(basket.Id),
                AnonymousId = basket.AnonymousId,
                CreatedAt = DateTime.Parse(basket.CreatedAt.ToString()),
                Items = new List<BasketItem>()
            };

            if (basket.CustomerId != null)
            {
                result.Customer = await _customerRepository.GetCustomerById(basket.CustomerId);
            }

            result.Items = await GetBasketItems(result.Id);

            return result;
        }

        public async Task<Basket> CreateBasket(Basket basket)
        {
            var query =
                "INSERT INTO Basket (CustomerId, AnonymousId, CreatedAt) " +
                "VALUES (@customerId, @anonymousId, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var basketId = await conn.QuerySingleAsync<int>(query, new
            {
                customerId = basket.Customer?.Id,
                anonymousId = basket.AnonymousId,
                createdAt = basket.CreatedAt
            });

            basket.Id = basketId;
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
                    newBasket.Customer = await _customerRepository.GetCustomerById(customerId.Value);
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
            var query = "DELETE FROM Basket WHERE Id = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { basketId });
        }

        public async Task RemoveAllBasketItems(int basketId)
        {
            var query = "DELETE FROM BasketItem WHERE BasketId = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { basketId });
        }

        public async Task<List<BasketItem>?> GetBasketItems(int basketId)
        {
            var query =
                "SELECT Id, BasketId, ProductId, Quantity, Price, VATRate, CreatedAt " +
                "FROM BasketItem " +
                "WHERE BasketId = @basketId";

            await using var conn = new SqliteConnection(ConnectionString);

            var basketItems = await conn.QueryAsync<dynamic>(query, new { basketId });

            var result = basketItems
                .Select(item => new BasketItem
                {
                    Id = Convert.ToInt32(item.Id),
                    BasketId = Convert.ToInt32(item.BasketId),
                    Product = new Product { Id = Convert.ToInt32(item.ProductId) },
                    Quantity = Convert.ToInt32(item.Quantity),
                    Price = item.Price,
                    VATRate = item.VATRate,
                    CreatedAt = DateTime.Parse(item.CreatedAt.ToString())
                })
                .ToList();

            foreach (var basketItem in result)
            {
                var product = await _productRepository.GetProduct(basketItem.Product.Id);
                basketItem.Product = product;
            }

            return result;
        }

        public async Task<BasketItem> CreateBasketItem(BasketItem basketItem)
        {
            var query =
                "INSERT INTO BasketItem (BasketId, ProductId, Quantity, Price, VATRate, CreatedAt) " +
                "VALUES (@basketId, @productId, @quantity, @price, @vatRate, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var basketItemId = await conn.QuerySingleAsync<int>(query, new
            {
                basketId = basketItem.BasketId,
                productId = basketItem.Product?.Id ?? 0,
                quantity = basketItem.Quantity,
                price = basketItem.Price,
                vatRate = basketItem.VATRate,
                createdAt = basketItem.CreatedAt
            });

            basketItem.Id = basketItemId;
            return basketItem;
        }

        public async Task UpdateBasketItemQuantity(int basketItemId, int quantity)
        {
            var query = "UPDATE BasketItem SET Quantity = @quantity WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { basketItemId, quantity });
        }

        public async Task DeleteBasketItem(int basketItemId)
        {
            var query = "DELETE FROM BasketItem WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { basketItemId });
        }

        public async Task<bool> BasketItemExists(int basketItemId)
        {
            var query = "SELECT COUNT(*) FROM BasketItem WHERE Id = @basketItemId";

            await using var conn = new SqliteConnection(ConnectionString);

            var count = await conn.QuerySingleAsync<int>(query, new { basketItemId });
            return count > 0;
        }
    }
}
