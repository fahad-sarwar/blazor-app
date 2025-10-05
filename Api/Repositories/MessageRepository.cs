using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class MessageRepository : RepositoryBase
    {
        public async Task<Message> CreateMessage(Message message)
        {
            var query =
                "INSERT INTO Message (Name, Email, Subject, Content, Processed, CreatedAt) " +
                "VALUES (@name, @email, @subject, @content, @processed, @createdAt); " +
                "SELECT last_insert_rowid() FROM Message LIMIT 1;";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@name", message.Name);
            command.Parameters.AddWithValue("@email", message.Email);
            command.Parameters.AddWithValue("@subject", message.Subject);
            command.Parameters.AddWithValue("@content", message.Content);
            command.Parameters.AddWithValue("@processed", message.Processed);
            command.Parameters.AddWithValue("@createdAt", message.CreatedAt.ToString("yyyy-MM-dd HH:mm:dd.fffffff"));

            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                throw new Exception("Failed to insert message.");

            while (reader.Read())
            {
                message.Id = reader.GetInt32(0);
            }

            return message;
        }
    }
}