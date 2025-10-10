using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class CustomerRepository(ProductRepository productRepository) : RepositoryBase
    {
        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await GetCustomerByField("Email", email);
        }

        public async Task<Customer?> GetCustomerByUserId(int userId)
        {
            return await GetCustomerByField("UserId", userId);
        }

        public async Task<Customer?> GetCustomerById(int customerId)
        {
            return await GetCustomerByField("Id", customerId);
        }

        private async Task<Customer?> GetCustomerByField(string fieldName, object fieldValue)
        {
            var query =
                "SELECT Id, FirstName, LastName, Email, PhoneNumber, UserId, CreatedAt, BillingAddressId, ShippingAddressId " +
                "FROM Customer " +
                $"WHERE {fieldName} = @fieldValue";

            await using var conn = new SqliteConnection(ConnectionString);

            var customerData = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { fieldValue = fieldValue });

            var customer = new Customer
            {
                Id = Convert.ToInt32(customerData.Id),
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                UserId = Convert.ToInt32(customerData.UserId),
                CreatedAt = DateTime.Parse(customerData.CreatedAt.ToString())
            };

            if (customerData.BillingAddressId != null)
            {
                customer.BillingAddress = await GetAddress(Convert.ToInt32(customerData.BillingAddressId));
            }

            if (customerData.ShippingAddressId != null)
            {
                customer.ShippingAddress = await GetAddress(Convert.ToInt32(customerData.ShippingAddressId));
            }

            return customer;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var query =
                "INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, UserId, CreatedAt, BillingAddressId, ShippingAddressId) " +
                "VALUES (@firstName, @lastName, @email, @phoneNumber, @userId, @createdAt, @billingAddressId, @shippingAddressId); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var customerId = await conn.QuerySingleAsync<int>(query, new
            {
                firstName = customer.FirstName,
                lastName = customer.LastName,
                email = customer.Email,
                phoneNumber = customer.PhoneNumber,
                userId = customer.UserId,
                createdAt = customer.CreatedAt,
                billingAddressId = customer.BillingAddress?.Id,
                shippingAddressId = customer.ShippingAddress?.Id
            });

            customer.Id = customerId;
            return customer;
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var query =
                "UPDATE Customer " +
                "SET FirstName = @firstName, " +
                "LastName = @lastName, " +
                "Email = @email, " +
                "PhoneNumber = @phoneNumber, " +
                "BillingAddressId = @billingAddressId, " +
                "ShippingAddressId = @shippingAddressId " +
                "WHERE Id = @id";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new
            {
                id = customer.Id,
                firstName = customer.FirstName,
                lastName = customer.LastName,
                email = customer.Email,
                phoneNumber = customer.PhoneNumber,
                billingAddressId = customer.BillingAddress?.Id,
                shippingAddressId = customer.ShippingAddress?.Id
            });
        }

        public async Task<Address> CreateAddress(Address address)
        {
            var query =
                "INSERT INTO Address (AddressLineOne, AddressLineTwo, Town, County, PostCode, Country) " +
                "VALUES (@addressLineOne, @addressLineTwo, @town, @county, @postCode, @country); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var addressId = await conn.QuerySingleAsync<int>(query, new
            {
                addressLineOne = address.AddressLineOne,
                addressLineTwo = address.AddressLineTwo,
                town = address.Town,
                county = address.County,
                postCode = address.PostCode,
                country = address.Country
            });

            address.Id = addressId;
            return address;
        }

        public async Task<Address?> GetAddress(int addressId)
        {
            Address? address = null;

            var query =
                "SELECT Id, AddressLineOne, AddressLineTwo, Town, County, PostCode, Country " +
                "FROM Address " +
                "WHERE Id = @addressId";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<Address>(query, new { addressId });
        }

        public async Task UpdateAddress(Address address)
        {
            var query =
                "UPDATE Address " +
                "SET AddressLineOne = @addressLineOne, " +
                "AddressLineTwo = @addressLineTwo, " +
                "Town = @town, " +
                "County = @county, " +
                "PostCode = @postCode, " +
                "Country = @country " +
                "WHERE Id = @id";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new
            {
                id = address.Id,
                addressLineOne = address.AddressLineOne,
                addressLineTwo = address.AddressLineTwo,
                town = address.Town,
                county = address.County,
                postCode = address.PostCode,
                country = address.Country
            });
        }

        public async Task<(List<Product> Products, int TotalCount)> GetWishlistProducts(int customerId, int page = 1, int pageSize = 10)
        {
            var countQuery = "SELECT COUNT(*) FROM Wishlist WHERE CustomerId = @customerId";

            var wishlistQuery =
                "SELECT ProductId " +
                "FROM Wishlist " +
                "WHERE CustomerId = @customerId " +
                "ORDER BY CreatedAt DESC " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);

            var totalCount = await conn.QuerySingleAsync<int>(countQuery, new { customerId });

            var productIds = await conn.QueryAsync<int>(wishlistQuery, new
            {
                customerId,
                pageSize,
                offset = (page - 1) * pageSize
            });

            var products = new List<Product>();

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

        public async Task<bool> IsProductInWishlist(int customerId, int productId)
        {
            var query =
                "SELECT COUNT(*) " +
                "FROM Wishlist " +
                "WHERE CustomerId = @customerId AND ProductId = @productId";

            await using var conn = new SqliteConnection(ConnectionString);

            var count = await conn.QuerySingleAsync<int>(query, new { customerId, productId });
            return count > 0;
        }

        public async Task AddToWishlist(int customerId, int productId)
        {
            var query =
                "INSERT INTO Wishlist (CustomerId, ProductId, CreatedAt) " +
                "VALUES (@customerId, @productId, @createdAt)";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new
            {
                customerId,
                productId,
                createdAt = DateTime.UtcNow
            });
        }

        public async Task RemoveFromWishlist(int customerId, int productId)
        {
            var query = "DELETE FROM Wishlist WHERE CustomerId = @customerId AND ProductId = @productId";

            await using var conn = new SqliteConnection(ConnectionString);

            await conn.ExecuteAsync(query, new { customerId, productId });
        }

        public async Task<(List<Review> Reviews, int TotalCount)> GetReviews(int productId, int page = 1, int pageSize = 10)
        {
            var countQuery = "SELECT COUNT(*) FROM Review WHERE ProductId = @productId AND Status = 'Approved'";

            var reviewsQuery =
                "SELECT r.Id, r.Subject, r.Rating, r.Comment, r.Status, r.ProductId, r.CustomerId, r.CreatedAt " +
                "FROM Review r " +
                "WHERE r.ProductId = @productId AND r.Status = 'Approved' " +
                "ORDER BY r.CreatedAt DESC " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);

            var totalCount = await conn.QuerySingleAsync<int>(countQuery, new { productId });

            var reviewData = await conn.QueryAsync<dynamic>(reviewsQuery, new
            {
                productId,
                pageSize,
                offset = (page - 1) * pageSize
            });

            var reviews = new List<Review>();

            foreach(var review in reviewData)
            {
                var r = new Review
                {
                    Id = Convert.ToInt32(review.Id),
                    Subject = review.Subject,
                    Rating = Convert.ToInt32(review.Rating),
                    Comment = review.Comment,
                    Status = review.Status,
                    Product = new Product { Id = Convert.ToInt32(review.ProductId) },
                    Customer = new Customer { Id = Convert.ToInt32(review.CustomerId) },
                    CreatedAt = DateTime.Parse(review.CreatedAt.ToString())
                };

                r.Customer = await GetCustomerById(r.Customer.Id);

                reviews.Add(r);
            }

            return (reviews, totalCount);
        }

        public async Task<double?> GetAverageRating(int productId)
        {
            var query = "SELECT AVG(CAST(Rating AS REAL)) FROM Review WHERE ProductId = @productId AND Status = 'Approved'";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QuerySingleOrDefaultAsync<double?>(query, new { productId });
        }

        public async Task<Review> CreateReview(Review review)
        {
            var query =
                "INSERT INTO Review (Subject, Rating, Comment, Status, ProductId, CustomerId, CreatedAt) " +
                "VALUES (@subject, @rating, @comment, @status, @productId, @customerId, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var reviewId = await conn.QuerySingleAsync<int>(query, new
            {
                subject = review.Subject,
                rating = review.Rating,
                comment = review.Comment,
                status = review.Status,
                productId = review.Product?.Id ?? 0,
                customerId = review.Customer?.Id ?? 0,
                createdAt = review.CreatedAt
            });

            review.Id = reviewId;
            return review;
        }
    }
}