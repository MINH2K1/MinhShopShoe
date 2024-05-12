using Microsoft.AspNetCore.Mvc;

namespace ShopShoe.WebApp.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
