using GitHubRepoViewer.Data;
using Microsoft.AspNetCore.Mvc;

public class HistoryController : Controller
{
    private readonly GithubrepoviewerDbContext _context;

    public HistoryController(GithubrepoviewerDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "Account");

        var userId = _context.Users
                             .Where(u => u.Username == username)
                             .Select(u => u.Id)
                             .FirstOrDefault();

        var history = _context.History
                              .Where(h => h.UserId == userId)
                              .OrderByDescending(h => h.CreatedAt)
                              .ToList();

        return View(history);
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var entry = _context.History.Find(id);
        if (entry != null)
        {
            _context.History.Remove(entry);
            _context.SaveChanges();
        }

        return RedirectToAction("Index"); // go back to history list
    }

}
