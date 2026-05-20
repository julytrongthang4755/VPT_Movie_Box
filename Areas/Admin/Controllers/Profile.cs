using Microsoft.AspNetCore.Mvc;

namespace VPT_Movie_Box.Controllers
{
    [Area("Admin")]
    public class Profile : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
