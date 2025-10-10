using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class TaxRateRepository : RepositoryBase
    {
        public async Task<TaxRate?> GetCurrentTaxRate()
        {
            var query =
                "SELECT Id, Name, Rate, EffectiveFrom, EffectiveTo " +
                "FROM TaxRate " +
                "WHERE EffectiveFrom <= @now " +
                "AND (EffectiveTo IS NULL OR EffectiveTo > @now) " +
                "ORDER BY EffectiveFrom DESC " +
                "LIMIT 1";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<TaxRate>(query, new { now = DateTime.UtcNow });
        }
    }
}