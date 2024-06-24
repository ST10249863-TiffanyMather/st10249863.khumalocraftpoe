using Microsoft.AspNetCore.Mvc;

namespace KhumaloCraft_Part2.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
