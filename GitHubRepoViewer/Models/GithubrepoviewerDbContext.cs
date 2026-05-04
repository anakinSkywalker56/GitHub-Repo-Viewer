using GitHubRepoViewer.Models;
using Microsoft.EntityFrameworkCore;

public class GithubrepoviewerDbContext : DbContext
{
    public GithubrepoviewerDbContext(DbContextOptions<GithubrepoviewerDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}
