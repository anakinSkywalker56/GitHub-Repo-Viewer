using System.Text.Json.Serialization;

namespace GitHubRepoViewer.Models
{
    public class Repo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("html_url")]
        public string Html_url { get; set; }
        [JsonPropertyName("stargazers_count")]
        public int Stargazers_count { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
