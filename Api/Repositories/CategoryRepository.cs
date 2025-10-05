using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class CategoryRepository : RepositoryBase
    {
        public async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();

            var query =
                "SELECT Id, Name, Description, CreatedAt " +
                "FROM Category;";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return categories;

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var description = reader.GetString(2);
                var createdAt = reader.GetDateTime(3);

                categories.Add(new Category
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    CreatedAt = createdAt
                });
            }

            return categories;
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            Category? category = null;

            var query =
                "SELECT Id, Name, Description, CreatedAt " +
                "FROM Category " +
                "WHERE Id = @categoryId;";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@categoryId", categoryId);

            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return category;

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var description = reader.GetString(2);
                var createdAt = reader.GetDateTime(3);

                category = new Category
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    CreatedAt = createdAt
                };
            }

            return category;
        }
    }
}
