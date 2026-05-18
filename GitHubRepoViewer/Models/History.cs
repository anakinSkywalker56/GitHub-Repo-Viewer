namespace GitHubRepoViewer.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserId { get; set; }      // FK to AspNetUsers or your session user
        public string? Query { get; set; }       // search string
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
