using Microsoft.AspNetCore.Mvc;

namespace api_demo.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
