using Microsoft.AspNetCore.Mvc;

namespace LostPaw.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
