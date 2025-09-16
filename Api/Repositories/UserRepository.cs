using Api.Models;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

namespace Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserName(string userName);
        Task<User?> GetUserById(int userId);
        Task<User> CreateUser(User user);
        Task<User> CreateUser(string userName, string password, bool isAdmin = false);
        Task<bool> UserExists(string userName);
        Task<bool> ValidatePassword(string userName, string password);
    }

    public class UserRepository(ILogger<UserRepository> logger) : RepositoryBase, IUserRepository
    {
        public async Task<User?> GetUserByUserName(string userName)
        {
            var query = 
                "SELECT Id, UserName, PasswordHash, IsAdmin, CreatedAt " +
                "FROM User " +
                "WHERE UserName = @userName";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@userName", userName);

                var reader = await command.ExecuteReaderAsync();

                User? user = null;

                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        IsAdmin = reader.GetBoolean(3),
                        CreatedAt = reader.GetDateTime(4)
                    };
                }

                return user;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<User?> GetUserById(int userId)
        {
            var query = 
                "SELECT Id, UserName, PasswordHash, IsAdmin, CreatedAt " +
                "FROM User " +
                "WHERE Id = @userId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync();

                User? user = null;

                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        IsAdmin = reader.GetBoolean(3),
                        CreatedAt = reader.GetDateTime(4)
                    };
                }

                return user;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<User> CreateUser(User user)
        {
            var query = 
                "INSERT INTO User (UserName, PasswordHash, IsAdmin, CreatedAt) " +
                "VALUES (@userName, @passwordHash, @isAdmin, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@userName", user.UserName);
                command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@isAdmin", user.IsAdmin);
                command.Parameters.AddWithValue("@createdAt", user.CreatedAt);

                var userId = await command.ExecuteScalarAsync();
                user.Id = Convert.ToInt32(userId);
                return user;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<User> CreateUser(string userName, string password, bool isAdmin = false)
        {
            var user = new User
            {
                UserName = userName,
                PasswordHash = HashPassword(password),
                IsAdmin = isAdmin,
                CreatedAt = DateTime.UtcNow
            };

            return await CreateUser(user);
        }

        public async Task<bool> UserExists(string userName)
        {
            var query =
                "SELECT COUNT(*) " +
                "FROM User " +
                "WHERE UserName = @userName";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@userName", userName);

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> ValidatePassword(string userName, string password)
        {
            var user = await GetUserByUserName(userName);

            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.PasswordHash);
        }

        // Password hashing methods
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var salt = GenerateSalt();
            var saltedPassword = password + salt;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hashedPassword = Convert.ToBase64String(hashedBytes);
            return $"{salt}:{hashedPassword}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2)
                    return false;

                var salt = parts[0];
                var hash = parts[1];

                using var sha256 = SHA256.Create();
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var computedHash = Convert.ToBase64String(hashedBytes);

                return hash == computedHash;
            }
            catch
            {
                return false;
            }
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}