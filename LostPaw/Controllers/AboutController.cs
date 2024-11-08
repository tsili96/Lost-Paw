using Microsoft.AspNetCore.Mvc;

namespace LostPaw.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
