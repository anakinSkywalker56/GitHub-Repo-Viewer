using Microsoft.AspNetCore.Mvc;
using GitHubRepoViewer.Services;
using GitHubRepoViewer.Models;

namespace GitHubRepoViewer.Controllers
{
    public class GitHubController : Controller
    {
        private readonly GitHubService _gitHubService;

        public GitHubController(GitHubService githubService)
        {
            _gitHubService = githubService;
        }

        public async Task<IActionResult> Index(string username = "octocat")
        {
            var repos = await _gitHubService.GetReposAsync(username);

            if (repos == null) {
                ViewBag.ErrorMessage = "User does not exist :(";
                return View(new List<Repo>());
            }

            return View(repos);
        }
    }
}
