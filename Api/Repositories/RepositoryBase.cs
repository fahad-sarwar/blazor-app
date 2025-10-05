using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class RepositoryBase
    {
        public static string ConnectionString = "Data Source=OnlineShopDb.db;";

        public async Task ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}
