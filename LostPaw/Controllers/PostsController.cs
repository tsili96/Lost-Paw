using Microsoft.AspNetCore.Mvc;

namespace LostPaw.Controllers
{
    public class PostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
