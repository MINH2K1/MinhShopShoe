using Microsoft.AspNetCore.Mvc;

namespace ShopShoe.WebApp.Areas.Admin.Controllers
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
            _logger.LogInformation("Loi", DateTime.UtcNow.ToShortTimeString());
            return View();
        }

    }
}
