using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class ReviewRepository(CustomerRepository customerRepository) : RepositoryBase
    {
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
                    var customer = await customerRepository.GetCustomerById(review.Customer.Id);
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

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@subject", review.Subject);
                command.Parameters.AddWithValue("@rating", review.Rating);
                command.Parameters.AddWithValue("@comment", review.Comment);
                command.Parameters.AddWithValue("@status", review.Status);
                command.Parameters.AddWithValue("@productId", review.Product?.Id ?? 0);
                command.Parameters.AddWithValue("@customerId", review.Customer?.Id ?? 0);
                command.Parameters.AddWithValue("@createdAt", review.CreatedAt);

                var reviewId = await command.ExecuteScalarAsync();
                review.Id = Convert.ToInt32(reviewId);
                return review;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}