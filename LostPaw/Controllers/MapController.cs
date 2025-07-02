using LostPaw.AppConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LostPaw.Controllers
{
    public class MapController : Controller
    {
        private readonly IOptions<GoogleConfig> googleConfig;

        public MapController(IOptions<GoogleConfig> googleConfig)
        {
            ArgumentNullException.ThrowIfNull(googleConfig, nameof(googleConfig));

            this.googleConfig = googleConfig;
        }
        public IActionResult Index()
        {
            return View(googleConfig.Value);
        }
    }
}
