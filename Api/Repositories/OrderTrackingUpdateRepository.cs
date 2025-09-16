using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IOrderTrackingUpdateRepository
    {
        Task<OrderTrackingUpdate> CreateTrackingUpdate(OrderTrackingUpdate trackingUpdate);
        Task<List<OrderTrackingUpdate>> GetTrackingUpdatesByOrderId(int orderId);
    }

    public class OrderTrackingUpdateRepository(ILogger<OrderTrackingUpdateRepository> logger) : RepositoryBase, IOrderTrackingUpdateRepository
    {
        public async Task<OrderTrackingUpdate> CreateTrackingUpdate(OrderTrackingUpdate trackingUpdate)
        {
            var query = 
                "INSERT INTO OrderTrackingUpdate (OrderId, UpdatedBy, Status, Note, CreatedAt) " +
                "VALUES (@orderId, @updatedBy, @status, @note, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", trackingUpdate.OrderId);
                command.Parameters.AddWithValue("@updatedBy", trackingUpdate.UpdatedBy);
                command.Parameters.AddWithValue("@status", trackingUpdate.Status);
                command.Parameters.AddWithValue("@note", trackingUpdate.Note);
                command.Parameters.AddWithValue("@createdAt", trackingUpdate.CreatedAt);

                var orderTrackingUpdateId = await command.ExecuteScalarAsync();
                trackingUpdate.Id = Convert.ToInt32(orderTrackingUpdateId);
                return trackingUpdate;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<List<OrderTrackingUpdate>> GetTrackingUpdatesByOrderId(int orderId)
        {
            var trackingUpdates = new List<OrderTrackingUpdate>();

            var query = 
                "SELECT Id, OrderId, UpdatedBy, Status, Note, CreatedAt " +
                "FROM OrderTrackingUpdate " +
                "WHERE OrderId = @orderId " +
                "ORDER BY CreatedAt ASC";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@orderId", orderId);

                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    trackingUpdates.Add(new OrderTrackingUpdate
                    {
                        Id = reader.GetInt32(0),
                        OrderId = reader.GetInt32(1),
                        UpdatedBy = reader.GetString(2),
                        Status = reader.GetString(3),
                        Note = reader.GetString(4),
                        CreatedAt = reader.GetDateTime(5)
                    });
                }

                return trackingUpdates;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}