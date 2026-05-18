using GitHubRepoViewer.Models;
using Microsoft.EntityFrameworkCore;

namespace GitHubRepoViewer.Data
{
    public class GithubrepoviewerDbContext : DbContext
    {
        public GithubrepoviewerDbContext(DbContextOptions<GithubrepoviewerDbContext> options)
            : base(options)
        {
        }

        public DbSet<History> History { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
