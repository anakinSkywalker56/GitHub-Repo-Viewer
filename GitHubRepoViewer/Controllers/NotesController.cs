using GitHubRepoViewer.Data;
using GitHubRepoViewer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GitHubRepoViewer.Controllers
{
    public class NotesController : Controller
    {
        private readonly GithubrepoviewerDbContext _context;

        public NotesController(GithubrepoviewerDbContext context)
        {
            _context = context;
        }

        // Show notes for a specific repo
        public IActionResult Index(string repoName)
        {
            if (string.IsNullOrEmpty(repoName))
                return RedirectToAction("Index", "GitHub");

            var notes = _context.Notes
                .Where(n => n.RepoName == repoName)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            ViewBag.RepoName = repoName;
            ViewBag.AutoEdit = notes.Any();
            return View(notes);
        }

        // Add a new note
        [HttpPost]
        public IActionResult Add(string repoName, string noteText)
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

            return RedirectToAction("Index", new { repoName });
        }

        // Delete a note
        [HttpPost]
        public IActionResult Delete(int id, string repoName)
        {
            var note = _context.Notes.Find(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { repoName });
        }
        // Edit
        [HttpPost]
        public IActionResult Edit(int id, string repoName)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
                return RedirectToAction("Index", new { repoName });

            // Pass the note text into ViewBag so the editor is pre-filled
            var notes = _context.Notes
                .Where(n => n.RepoName == repoName)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            ViewBag.RepoName = repoName;
            ViewBag.AutoEdit = true;
            ViewBag.EditNoteId = id;
            ViewBag.EditNoteText = note.NoteText;

            return View("Index", notes);
        }

        [HttpPost]
        public IActionResult Save(int? id, string repoName, string noteText)
        {
            if (string.IsNullOrEmpty(repoName) || string.IsNullOrEmpty(noteText))
                return RedirectToAction("Index", new { repoName });

            if (id.HasValue)
            {
                // Update existing note
                var note = _context.Notes.Find(id.Value);
                if (note != null)
                {
                    note.NoteText = noteText;
                    _context.SaveChanges();
                }
            }
            else
            {
                // Add new note
                var note = new Note
                {
                    RepoName = repoName,
                    NoteText = noteText,
                    CreatedAt = DateTime.Now
                };
                _context.Notes.Add(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { repoName });
        }

    }
}
