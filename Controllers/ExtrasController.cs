using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Super_Simple_Homebrew_Hoster.Models;

namespace Super_Simple_Homebrew_Hoster.Controllers
{
    public class ExtrasController : Controller
    {
        private readonly ILogger<ExtrasController> _logger;

        public ExtrasController(ILogger<ExtrasController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
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
