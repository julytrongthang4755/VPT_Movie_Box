using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VPT_Movie_Box.Models;

namespace VPT_Movie_Box.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Controllers/HomeController.cs
        public IActionResult FormElement()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult Movies()
        {
            return RedirectToAction("Index", "Movies");
        }

        public IActionResult TransHis()
        {
            return RedirectToAction("Index", "TransactionHistory");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
