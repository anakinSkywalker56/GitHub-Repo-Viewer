namespace GitHubRepoViewer.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int? RepoId { get; set; }
        public string? TargetUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
