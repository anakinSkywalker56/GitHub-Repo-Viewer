namespace GitHubRepoViewer.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string RepoName { get; set; } = "";
        public string NoteText { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

}
