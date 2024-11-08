using Microsoft.AspNetCore.Mvc;

namespace LostPaw.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
