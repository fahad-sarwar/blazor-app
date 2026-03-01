using Microsoft.Data.Sqlite;

namespace Api.Data
{
    public class DatabaseBuilder
    {
        private readonly ILogger<DatabaseBuilder> _logger;

        public DatabaseBuilder(ILogger<DatabaseBuilder> logger)
        {
            _logger = logger;
        }

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
                _logger.LogError(ex, "Error setting up the database");
                throw;
            }
        }

        private void DeleteExistingDatabase()
        {
            _logger.LogInformation("Deleting existing database file if it exists.");

            if (File.Exists(Filename))
            {
                File.Delete(Filename);
                _logger.LogInformation("Existing database file deleted.");
            }
        }

        private async Task BuildDatabaseSchema()
        {
            _logger.LogInformation("Creating database schema.");

            var schema = await File.ReadAllTextAsync("Data/DatabaseSchema.sql");

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(schema, conn);
            conn.Open();

            await command.ExecuteNonQueryAsync();

            _logger.LogInformation("Created database schema.");
        }

        private async Task CreateTestData()
        {
            _logger.LogInformation("Creating test data.");

            var schema = await File.ReadAllTextAsync("Data/TestData.sql");

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(schema, conn);
            conn.Open();

            await command.ExecuteNonQueryAsync();

            _logger.LogInformation("Created test data.");
        }
    }
}
