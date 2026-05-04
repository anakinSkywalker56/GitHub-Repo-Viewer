using System.Net.Http.Headers;
using System.Text.Json;
using GitHubRepoViewer.Models;

namespace GitHubRepoViewer.Services
{
    public class GitHubService
    {
        private readonly HttpClient _httpClient;

        public GitHubService(HttpClient httpClient) { 
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("RepoViewer", "1.0"));
        }

        public async Task<List<Repo>> GetReposAsync(string username)
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/users/{username}/repos");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                
                return null;
            }
            
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Repo>>(json);
        }
    }
}
