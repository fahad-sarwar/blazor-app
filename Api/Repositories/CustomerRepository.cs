using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerByEmail(string email);
        Task<Customer?> GetCustomerByUserId(string userId);
        Task<Customer?> GetCustomerById(int customerId);
        Task<Customer> CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task UpdateCustomerPhoneNumber(Customer customer, string phoneNumber);
        Task<bool> CustomerExists(int customerId);
    }

    public class CustomerRepository(ILogger<CustomerRepository> logger, IAddressRepository addressRepository) : RepositoryBase, ICustomerRepository
    {
        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await GetCustomerByField("Email", email);
        }

        public async Task<Customer?> GetCustomerByUserId(string userId)
        {
            return await GetCustomerByField("UserId", userId);
        }

        public async Task<Customer?> GetCustomerById(int customerId)
        {
            return await GetCustomerByField("Id", customerId);
        }

        private async Task<Customer?> GetCustomerByField(string fieldName, object fieldValue)
        {
            var query = 
                "SELECT Id, FirstName, LastName, Email, PhoneNumber, UserId, CreatedAt, BillingAddressId, ShippingAddressId " +
                "FROM Customer " +
                $"WHERE {fieldName} = @fieldValue";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@fieldValue", fieldValue);

                var reader = await command.ExecuteReaderAsync();

                Customer? customer = null;

                if (reader.Read())
                {
                    var billingAddressId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7);
                    var shippingAddressId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);

                    customer = new Customer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        UserId = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };

                    reader.Close();

                    if (billingAddressId.HasValue)
                    {
                        customer.BillingAddress = await addressRepository.GetAddress(billingAddressId.Value);
                    }

                    if (shippingAddressId.HasValue)
                    {
                        customer.ShippingAddress = await addressRepository.GetAddress(shippingAddressId.Value);
                    }
                }

                return customer;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var query =
                "INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, UserId, CreatedAt, BillingAddressId, ShippingAddressId) " +
                "VALUES (@firstName, @lastName, @email, @phoneNumber, @userId, @createdAt, @billingAddressId, @shippingAddressId); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@email", customer.Email);
                command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber ?? string.Empty);
                command.Parameters.AddWithValue("@userId", customer.UserId);
                command.Parameters.AddWithValue("@createdAt", customer.CreatedAt);
                command.Parameters.AddWithValue("@billingAddressId", (object?)customer.BillingAddress?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@shippingAddressId", (object?)customer.ShippingAddress?.Id ?? DBNull.Value);

                var insertedId = await command.ExecuteScalarAsync();
                customer.Id = Convert.ToInt32(insertedId);
                return customer;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var query = 
                "UPDATE Customer " +
                "SET FirstName = @firstName, " +
                "LastName = @lastName, " +
                "Email = @email, " +
                "PhoneNumber = @phoneNumber, " +
                "BillingAddressId = @billingAddressId, " +
                "ShippingAddressId = @shippingAddressId " +
                "WHERE Id = @id";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@id", customer.Id);
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@email", customer.Email);
                command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber ?? string.Empty);
                command.Parameters.AddWithValue("@billingAddressId", (object?)customer.BillingAddress?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@shippingAddressId", (object?)customer.ShippingAddress?.Id ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
        public async Task UpdateCustomerPhoneNumber(Customer customer, string phoneNumber)
        {
            var query =
                "UPDATE Customer " +
                "SET PhoneNumber = @phoneNumber " +
                "WHERE Id = @id";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@id", customer.Id);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            var query = 
                "SELECT COUNT(*) " +
                "FROM Customer " +
                "WHERE Id = @customerId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@customerId", customerId);

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}