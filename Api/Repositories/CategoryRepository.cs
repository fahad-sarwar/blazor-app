using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class CategoryRepository : RepositoryBase
    {
        public async Task<List<Category>> GetCategories()
        {
            var query = "SELECT Id, Name, Description, CreatedAt FROM Category;";

            await using var conn = new SqliteConnection(ConnectionString);

            var categories = await conn.QueryAsync<Category>(query);
            return categories.ToList();
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            var query = "SELECT Id, Name, Description, CreatedAt FROM Category WHERE Id = @categoryId;";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<Category>(query, new { categoryId });
        }
    }
}
