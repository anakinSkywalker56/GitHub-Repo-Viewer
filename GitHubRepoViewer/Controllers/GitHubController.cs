using GitHubRepoViewer.Data;
using GitHubRepoViewer.Models;
using GitHubRepoViewer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GitHubRepoViewer.Controllers
{
    public class GitHubController : Controller
    {
        private readonly GitHubService _gitHubService;
        private readonly FavoritesService _favoritesService;
        private readonly GithubrepoviewerDbContext _context;

        public GitHubController(GitHubService githubService, FavoritesService favoritesService, GithubrepoviewerDbContext context)
        {
            _gitHubService = githubService;
            _favoritesService = favoritesService;
            _context = context;
        }

        public async Task<IActionResult> Index(string username = "octocat")
        {
            var repos = await _gitHubService.GetReposAsync(username);

            if (repos == null)
            {
                ViewBag.ErrorMessage = "User does not exist :(";
                ViewBag.SearchText = username; // preserve text
                HttpContext.Session.SetString("LastSearchUser", username); // save internally
                ViewBag.IsFavorite = false;
                return View(new List<Repo>());
            }

            // Save search text for reuse
            ViewBag.SearchText = username;
            HttpContext.Session.SetString("LastSearchUser", username);


            var repoNotes = _context.Notes.ToList();
            var currentUser = HttpContext.Session.GetString("Username");
            var favorites = await _favoritesService.GetFavorites(currentUser);
            ViewBag.RepoNotes = repoNotes;
            ViewBag.IsFavorite = favorites.Any(f => f.TargetUser == username);

            // ✅ Save to History automatically
            if (!string.IsNullOrEmpty(currentUser))
            {
                var userId = _context.Users
                    .Where(u => u.Username == currentUser)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                if (userId != 0) // ensure valid user
                {
                    var history = new History
                    {
                        UserId = userId,
                        Query = username, // targetUser from session
                        CreatedAt = DateTime.Now
                    };

                    _context.History.Add(history);
                    await _context.SaveChangesAsync();
                }
            }

            return View(repos);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleFavorite()
        {
            var currentUser = HttpContext.Session.GetString("Username");
            var targetUser = HttpContext.Session.GetString("LastSearchUser");

            if (string.IsNullOrEmpty(currentUser))
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrEmpty(targetUser))
                await _favoritesService.ToggleFavoriteUser(currentUser, targetUser);

            return RedirectToAction("Index", new { username = targetUser });
        }

        [HttpPost]
        public IActionResult AddNote(string repoName, string noteText)
        {
            if (!string.IsNullOrEmpty(repoName) && !string.IsNullOrEmpty(noteText))
            {
                var note = new Note
                {
                    RepoName = repoName,
                    NoteText = noteText,
                    CreatedAt = DateTime.Now
                };

                _context.Notes.Add(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Note", new { repoName });
        }

    }
}
