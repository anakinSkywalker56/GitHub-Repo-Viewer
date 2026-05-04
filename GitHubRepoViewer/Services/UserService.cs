using Microsoft.Data.SqlClient;
using Dapper;
using BCrypt.Net;
using GitHubRepoViewer.Models;


namespace GitHubRepoViewer.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT Id, Username, Password, Access_Level FROM Users WHERE Username = @Username",
                new { Username = username });

            if (user == null)
                return null;

            // Verify bcrypt hash
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isValid)
                return null;

            return user; // contains Username + AccessLevel
        }

        public async Task<bool> Register(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);

            // Check if user already exists
            var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });

            if (existingUser != null)
                return false;

            // Hash password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Insert new user
            var rows = await connection.ExecuteAsync(
                "INSERT INTO Users (Username, Password, Access_Level) VALUES (@Username, @Password, @AccessLevel)",
                new { Username = username, Password = hashedPassword, AccessLevel = "User" });

            return rows > 0;
        }

    }
}
