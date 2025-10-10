using Api.Models;
using Dapper;
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

            var messageId = await conn.QuerySingleAsync<int>(query, new
            {
                name = message.Name,
                email = message.Email,
                subject = message.Subject,
                content = message.Content,
                processed = message.Processed,
                createdAt = message.CreatedAt.ToString("yyyy-MM-dd HH:mm:dd.fffffff")
            });

            message.Id = messageId;
            return message;
        }
    }
}