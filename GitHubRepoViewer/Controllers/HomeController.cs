using System.Diagnostics;
using GitHubRepoViewer.Models;
using Microsoft.AspNetCore.Mvc;
using GitHubRepoViewer.Data;

namespace GitHubRepoViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GithubrepoviewerDbContext _db;

        public HomeController(ILogger<HomeController> logger, GithubrepoviewerDbContext context)
        {
            _logger = logger;
            _db = context;
        }

        public IActionResult Index()
        {
            var user = _db.Users.FirstOrDefault();
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
