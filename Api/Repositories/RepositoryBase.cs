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
            try
            {
                conn.Open();

                foreach(var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
