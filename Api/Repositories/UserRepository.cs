using Api.Configuration;
using Api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Dapper;

namespace Api.Repositories
{
    public class UserRepository : RepositoryBase
    {
        private readonly PasswordConfiguration _passwordConfig;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, IOptions<PasswordConfiguration> passwordConfiguration)
        {
            _logger = logger;
            _passwordConfig = passwordConfiguration.Value;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var query = "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt FROM User WHERE Username = @username";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<User>(query, new { username });
        }

        public async Task<User?> GetUserById(int userId)
        {
            var query = "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt FROM User WHERE Id = @userId";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<User>(query, new { userId });
        }

        public async Task<User> CreateUser(string username, string password, bool isAdmin = false)
        {
            var now = DateTime.UtcNow;
            var passwordHash = HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                IsAdmin = isAdmin,
                CreatedAt = now
            };

            var query =
                "INSERT INTO User (Username, PasswordHash, IsAdmin, CreatedAt) " +
                "VALUES (@username, @passwordHash, @isAdmin, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var userId = await conn.QuerySingleAsync<int>(query, new
            {
                username,
                passwordHash,
                isAdmin,
                createdAt = now
            });

            return new User
            {
                Id = userId,
                Username = username,
                PasswordHash = passwordHash,
                IsAdmin = isAdmin,
                CreatedAt = now
            };
        }

        public async Task<bool> UserExists(string username)
        {
            var query = "SELECT COUNT(*) FROM User WHERE Username = @username";

            await using var conn = new SqliteConnection(ConnectionString);

            var count = await conn.QuerySingleAsync<int>(query, new { username });
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
                _logger.LogWarning(ex, "Password verification failed");
                return false;
            }
        }
    }
}