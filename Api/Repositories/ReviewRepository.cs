using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IReviewRepository
    {
        Task<(List<Review> Reviews, int TotalCount)> GetReviews(int productId, int page = 1, int pageSize = 10);
        Task<double?> GetAverageRating(int productId);
        Task<Review> CreateReview(Review review);
    }

    public class ReviewRepository(ILogger<ReviewRepository> logger, ICustomerRepository customerRepository) : RepositoryBase, IReviewRepository
    {
        public async Task<(List<Review> Reviews, int TotalCount)> GetReviews(int productId, int page = 1, int pageSize = 10)
        {
            var reviews = new List<Review>();

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

                var reviewData = new List<(int Id, string Subject, int Rating, string Comment, string Status, int ProductId, int CustomerId, DateTime CreatedAt)>();

                while (reader.Read())
                {
                    reviewData.Add((
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetInt32(5),
                        reader.GetInt32(6),
                        reader.GetDateTime(7)
                    ));
                }

                reader.Close();

                foreach (var data in reviewData)
                {
                    var customer = await customerRepository.GetCustomerById(data.CustomerId);
                    
                    if (customer != null)
                    {
                        reviews.Add(new Review
                        {
                            Id = data.Id,
                            Subject = data.Subject,
                            Rating = data.Rating,
                            Comment = data.Comment,
                            Status = data.Status,
                            Customer = customer,
                            CreatedAt = data.CreatedAt
                        });
                    }
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