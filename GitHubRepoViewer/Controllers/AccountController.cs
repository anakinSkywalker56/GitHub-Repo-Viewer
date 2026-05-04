using GitHubRepoViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubRepoViewer.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View(); // Views/Account/Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validate credentials against DB here...
            var user = await _userService.Authenticate(username, password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }

            // Store role in session
            HttpContext.Session.SetString("AccessLevel", user.AccessLevel);
            HttpContext.Session.SetString("Username", user.Username);

            // ✅ Redirect ALL users to UserInfo
            return RedirectToAction("UserInfo", "Home");
        }


        public IActionResult Register()
        {
            return View(); // Views/Account/Register.cshtml
        }
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match";
                return View();
            }

            var result = await _userService.Register(username, password);

            if (!result)
            {
                ViewBag.ErrorMessage = "Registration failed. Username may already exist.";
                return View();
            }

            // ✅ After successful registration, redirect to Login
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            // Clear session/cookies here
            return RedirectToAction("Login");
        }
    }
}
