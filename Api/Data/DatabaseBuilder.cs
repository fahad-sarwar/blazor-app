using Microsoft.Data.Sqlite;

namespace Api.Data
{
    public class DatabaseBuilder(ILogger<DatabaseBuilder> logger)
    {
        private const string ConnectionString = "Data Source=OnlineShopDb.db;";
        private const string Filename = "OnlineShopDb.db";

        public async Task SetupDatabase()
        {
            try
            {
                DeleteExistingDatabase();
                await BuildDatabaseSchema();
                await CreateTestData();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting up the database");
                throw;
            }
        }

        private void DeleteExistingDatabase()
        {
            logger.LogInformation("Deleting existing database file if it exists.");

            if (File.Exists(Filename))
            {
                File.Delete(Filename);
                logger.LogInformation("Existing database file deleted.");
            }
        }

        private async Task BuildDatabaseSchema()
        {
            logger.LogInformation("Creating database schema.");

            var schema = await File.ReadAllTextAsync("Data/DatabaseSchema.sql");

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(schema, conn);
            conn.Open();

            await command.ExecuteNonQueryAsync();

            logger.LogInformation("Created database schema.");
        }

        private async Task CreateTestData()
        {
            logger.LogInformation("Creating test data.");

            var schema = await File.ReadAllTextAsync("Data/TestData.sql");

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(schema, conn);
            conn.Open();

            await command.ExecuteNonQueryAsync();

            logger.LogInformation("Created test data.");
        }
    }
}
