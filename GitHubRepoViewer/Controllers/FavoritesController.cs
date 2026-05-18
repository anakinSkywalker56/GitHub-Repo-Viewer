using GitHubRepoViewer.Data;
using GitHubRepoViewer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GitHubRepoViewer.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly GithubrepoviewerDbContext _context;

        public FavoritesController(GithubrepoviewerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(currentUser))
                return RedirectToAction("Login", "Account");

            var favorites = _context.Favorites
                .Where(f => f.Username == currentUser)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();

            return View(favorites);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entry = _context.Favorites.Find(id);
            if (entry != null)
            {
                _context.Favorites.Remove(entry);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
