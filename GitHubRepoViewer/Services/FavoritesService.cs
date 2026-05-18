using Microsoft.Data.SqlClient;
using GitHubRepoViewer.Models;
using Dapper;

namespace GitHubRepoViewer.Services
{
    public class FavoritesService
    {
        private readonly string _connectionString;

        public FavoritesService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task ToggleFavoriteUser(string username, string targetUser)
        {
            using var connection = new SqlConnection(_connectionString);

            var existing = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Favorites WHERE Username = @Username AND TargetUser = @TargetUser",
                new { Username = username, TargetUser = targetUser });

            if (existing > 0)
            {
                await connection.ExecuteAsync(
                    "DELETE FROM Favorites WHERE Username = @Username AND TargetUser = @TargetUser",
                    new { Username = username, TargetUser = targetUser });
            }
            else
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Favorites (Username, TargetUser, CreatedAt) VALUES (@Username, @TargetUser, GETDATE())",
                    new { Username = username, TargetUser = targetUser });
            }
        }

        public async Task<IEnumerable<Favorite>> GetFavorites(string username)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<Favorite>(
                "SELECT * FROM Favorites WHERE Username = @Username",
                new { Username = username });
        }
    }

}
