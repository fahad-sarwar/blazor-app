using Api.Models;
using Microsoft.Data.Sqlite;
using System.Net;

namespace Api.Repositories
{
    public class CustomerRepository(ProductRepository productRepository) : RepositoryBase
    {
        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await GetCustomerByField("Email", email);
        }

        public async Task<Customer?> GetCustomerByUserId(string userId)
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
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@fieldValue", fieldValue);

                var reader = await command.ExecuteReaderAsync();

                Customer? customer = null;

                if (reader.Read())
                {
                    var billingAddressId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7);
                    var shippingAddressId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);

                    customer = new Customer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        UserId = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };

                    reader.Close();

                    if (billingAddressId.HasValue)
                    {
                        customer.BillingAddress = await GetAddress(billingAddressId.Value);
                    }

                    if (shippingAddressId.HasValue)
                    {
                        customer.ShippingAddress = await GetAddress(shippingAddressId.Value);
                    }
                }

                return customer;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var query =
                "INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, UserId, CreatedAt, BillingAddressId, ShippingAddressId) " +
                "VALUES (@firstName, @lastName, @email, @phoneNumber, @userId, @createdAt, @billingAddressId, @shippingAddressId); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@firstName", customer.FirstName },
                { "@lastName", customer.LastName },
                { "@email", customer.Email },
                { "@phoneNumber", customer.PhoneNumber },
                { "@userId", customer.UserId },
                { "@createdAt", customer.CreatedAt },
                { "@billingAddressId", (object?)customer.BillingAddress?.Id ?? DBNull.Value },
                { "@shippingAddressId", (object?)customer.ShippingAddress?.Id ?? DBNull.Value },
            };

            customer.Id = await ExecuteScalar(query, parameters);
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

            var parameters = new Dictionary<string, object>
            {
                { "@id", customer.Id },
                { "@firstName", customer.FirstName },
                { "@lastName", customer.LastName },
                { "@email", customer.Email },
                { "@phoneNumber", customer.PhoneNumber },
                { "@billingAddressId", (object?)customer.BillingAddress?.Id ?? DBNull.Value },
                { "@shippingAddressId", (object?)customer.ShippingAddress?.Id ?? DBNull.Value },
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task<Address> CreateAddress(Address address)
        {
            var query =
                "INSERT INTO Address (AddressLineOne, AddressLineTwo, Town, County, PostCode, Country) " +
                "VALUES (@addressLineOne, @addressLineTwo, @town, @county, @postCode, @country); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@addressLineOne", address.AddressLineOne },
                { "@addressLineTwo", address.AddressLineTwo },
                { "@town", address.Town },
                { "@county", address.County },
                { "@postCode", address.PostCode },
                { "@country", address.Country },
            };

            address.Id = await ExecuteScalar(query, parameters);
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
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@addressId", addressId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    address = new Address
                    {
                        Id = reader.GetInt32(0),
                        AddressLineOne = reader.GetString(1),
                        AddressLineTwo = reader.GetString(2),
                        Town = reader.GetString(3),
                        County = reader.GetString(4),
                        PostCode = reader.GetString(5),
                        Country = reader.GetString(6)
                    };
                }

                return address;
            }
            finally
            {
                conn.Close();
            }
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

            var parameters = new Dictionary<string, object>
            {
                { "@id", address.Id },
                { "@addressLineOne", address.AddressLineOne },
                { "@addressLineTwo", address.AddressLineTwo },
                { "@town", address.Town },
                { "@county", address.County },
                { "@postCode", address.PostCode },
                { "@country", address.Country },
            };

            await ExecuteNonQuery(query, parameters);
        }

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

                var countParameters = new Dictionary<string, object>
                {
                    { "@customerId", customerId }
                };

                var totalCount = await ExecuteScalar(countQuery, countParameters);

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

            var parameters = new Dictionary<string, object>
            {
                { "@customerId", customerId },
                { "@productId", productId }
            };

            var count = await ExecuteScalar(query, parameters);
            return count > 0;
        }

        public async Task AddToWishlist(int customerId, int productId)
        {
            var query =
                "INSERT INTO Wishlist (CustomerId, ProductId, CreatedAt) " +
                "VALUES (@customerId, @productId, @createdAt)";

            var parameters = new Dictionary<string, object>
            {
                { "@customerId", customerId },
                { "@productId", productId },
                { "@createdAt", DateTime.UtcNow }
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task RemoveFromWishlist(int customerId, int productId)
        {
            var query =
                "DELETE FROM Wishlist " +
                "WHERE CustomerId = @customerId AND ProductId = @productId";

            var parameters = new Dictionary<string, object>
            {
                { "@customerId", customerId },
                { "@productId", productId }
            };

            await ExecuteNonQuery(query, parameters);
        }

        public async Task<(List<Review> Reviews, int TotalCount)> GetReviews(int productId, int page = 1, int pageSize = 10)
        {
            var countQuery =
                "SELECT COUNT(*) " +
                "FROM Review " +
                "WHERE ProductId = @productId AND Status = 'Approved'";

            var reviewsQuery =
                "SELECT r.Id, r.Subject, r.Rating, r.Comment, r.Status, r.ProductId, r.CustomerId, r.CreatedAt " +
                "FROM Review r " +
                "WHERE r.ProductId = @productId AND r.Status = 'Approved' " +
                "ORDER BY r.CreatedAt DESC " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);
            try
            {
                conn.Open();

                await using var countCommand = new SqliteCommand(countQuery, conn);
                countCommand.Parameters.AddWithValue("@productId", productId);
                var totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());

                await using var reviewsCommand = new SqliteCommand(reviewsQuery, conn);
                reviewsCommand.Parameters.AddWithValue("@productId", productId);
                reviewsCommand.Parameters.AddWithValue("@pageSize", pageSize);
                reviewsCommand.Parameters.AddWithValue("@offset", (page - 1) * pageSize);

                var reader = await reviewsCommand.ExecuteReaderAsync();

                var reviews = new List<Review>();

                while (reader.Read())
                {
                    reviews.Add(new Review
                    {
                        Id = reader.GetInt32(0),
                        Subject = reader.GetString(1),
                        Rating = reader.GetInt32(2),
                        Comment = reader.GetString(3),
                        Status = reader.GetString(4),
                        Product = new Product { Id = reader.GetInt32(5) },
                        Customer = new Customer { Id = reader.GetInt32(6) },
                        CreatedAt = reader.GetDateTime(7)
                    });
                }

                reader.Close();

                foreach (var review in reviews)
                {
                    var customer = await GetCustomerById(review.Customer.Id);
                    review.Customer = customer;
                }

                return (reviews, totalCount);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<double?> GetAverageRating(int productId)
        {
            var query =
                "SELECT AVG(CAST(Rating AS REAL)) " +
                "FROM Review " +
                "WHERE ProductId = @productId AND Status = 'Approved'";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@productId", productId);

                var averageRating = await command.ExecuteScalarAsync();

                if (averageRating == null || averageRating == DBNull.Value)
                    return null;

                return Convert.ToDouble(averageRating);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Review> CreateReview(Review review)
        {
            var query =
                "INSERT INTO Review (Subject, Rating, Comment, Status, ProductId, CustomerId, CreatedAt) " +
                "VALUES (@subject, @rating, @comment, @status, @productId, @customerId, @createdAt); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@subject", review.Subject },
                { "@rating", review.Rating },
                { "@comment", review.Comment },
                { "@status", review.Status },
                { "@productId", review.Product?.Id ?? 0 },
                { "@customerId", review.Customer?.Id ?? 0 },
                { "@createdAt", review.CreatedAt },
            };

            review.Id = await ExecuteScalar(query, parameters);
            return review;
        }
    }
}