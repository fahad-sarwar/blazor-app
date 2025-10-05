using Api.Configuration;
using Api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Api.Repositories
{
    public class UserRepository(ILogger<UserRepository> logger, IOptions<PasswordConfiguration> passwordConfiguration) : RepositoryBase
    {
        private readonly PasswordConfiguration _passwordConfig = passwordConfiguration.Value;
        public async Task<User?> GetUserByUsername(string username)
        {
            var query =
                "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt " +
                "FROM User " +
                "WHERE Username = @username";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@username", username);

            var reader = await command.ExecuteReaderAsync();

            User? user = null;

            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    IsAdmin = reader.GetBoolean(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return user;
        }

        public async Task<User?> GetUserById(int userId)
        {
            var query =
                "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt " +
                "FROM User " +
                "WHERE Id = @userId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            conn.Open();

            command.Parameters.AddWithValue("@userId", userId);

            var reader = await command.ExecuteReaderAsync();

            User? user = null;

            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    IsAdmin = reader.GetBoolean(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return user;
        }

        public async Task<User> CreateUser(string username, string password, bool isAdmin = false)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                IsAdmin = isAdmin,
                CreatedAt = DateTime.UtcNow
            };

            var query =
                "INSERT INTO User (Username, PasswordHash, IsAdmin, CreatedAt) " +
                "VALUES (@username, @passwordHash, @isAdmin, @createdAt); " +
                "SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@username", user.Username },
                { "@passwordHash", user.PasswordHash },
                { "@isAdmin", user.IsAdmin },
                { "@createdAt", user.CreatedAt },
            };

            user.Id = await ExecuteScalar(query, parameters);
            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            var query =
                "SELECT COUNT(*) " +
                "FROM User " +
                "WHERE Username = @username";

            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
            };

            var count = await ExecuteScalar(query, parameters);
            return count > 0;
        }

        private string HashPassword(string password)
        {
            var saltBytes = Encoding.UTF8.GetBytes(_passwordConfig.Salt);

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public async Task<bool> ValidatePassword(string username, string password)
        {
            var user = await GetUserByUsername(username);

            if (user == null)
            {
                return false;
            }

            try
            {
                var enteredPasswordHash = HashPassword(password);
                return user.PasswordHash == enteredPasswordHash;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Password verification failed");
                return false;
            }
        }
    }
}