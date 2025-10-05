using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class TaxRateRepository : RepositoryBase
    {
        public async Task<TaxRate?> GetCurrentTaxRate()
        {
            TaxRate? taxRate = null;

            var query =
                "SELECT Id, Name, Rate, EffectiveFrom, EffectiveTo " +
                "FROM TaxRate " +
                "WHERE EffectiveFrom <= @now " +
                "AND (EffectiveTo IS NULL OR EffectiveTo > @now) " +
                "ORDER BY EffectiveFrom DESC " +
                "LIMIT 1";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@now", DateTime.UtcNow);

            var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                taxRate = new TaxRate
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Rate = reader.GetDouble(2),
                    EffectiveFrom = reader.GetDateTime(3),
                    EffectiveTo = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                };
            }

            return taxRate;
        }
    }
}