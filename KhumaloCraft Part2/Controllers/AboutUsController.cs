using Microsoft.AspNetCore.Mvc;

namespace KhumaloCraft_Part2.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
