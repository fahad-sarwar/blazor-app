using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class AddressRepository : RepositoryBase
    {
        public async Task<Address> CreateAddress(Address address)
        {
            var query = 
                "INSERT INTO Address (AddressLineOne, AddressLineTwo, Town, County, PostCode, Country) " +
                "VALUES (@addressLineOne, @addressLineTwo, @town, @county, @postCode, @country); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@addressLineOne", address.AddressLineOne);
                command.Parameters.AddWithValue("@addressLineTwo", address.AddressLineTwo ?? string.Empty);
                command.Parameters.AddWithValue("@town", address.Town);
                command.Parameters.AddWithValue("@county", address.County ?? string.Empty);
                command.Parameters.AddWithValue("@postCode", address.PostCode);
                command.Parameters.AddWithValue("@country", address.Country);

                var insertedId = await command.ExecuteScalarAsync();
                address.Id = Convert.ToInt32(insertedId);
                return address;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Address?> GetAddress(int addressId)
        {
            Address? address = null;

            var query = 
                "SELECT Id, AddressLineOne, AddressLineTwo, Town, County, PostCode, Country " +
                "FROM Address " +
                "WHERE Id = @addressId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@addressId", addressId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    address = new Address
                    {
                        Id = reader.GetInt32(0),
                        AddressLineOne = reader.GetString(1),
                        AddressLineTwo = reader.GetString(2),
                        Town = reader.GetString(3),
                        County = reader.GetString(4),
                        PostCode = reader.GetString(5),
                        Country = reader.GetString(6)
                    };
                }

                return address;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task UpdateAddress(Address address)
        {
            var query = 
                "UPDATE Address " +
                "SET AddressLineOne = @addressLineOne, " +
                "AddressLineTwo = @addressLineTwo, " +
                "Town = @town, " +
                "County = @county, " +
                "PostCode = @postCode, " +
                "Country = @country " +
                "WHERE Id = @id";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@id", address.Id);
                command.Parameters.AddWithValue("@addressLineOne", address.AddressLineOne);
                command.Parameters.AddWithValue("@addressLineTwo", address.AddressLineTwo ?? string.Empty);
                command.Parameters.AddWithValue("@town", address.Town);
                command.Parameters.AddWithValue("@county", address.County ?? string.Empty);
                command.Parameters.AddWithValue("@postCode", address.PostCode);
                command.Parameters.AddWithValue("@country", address.Country);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}