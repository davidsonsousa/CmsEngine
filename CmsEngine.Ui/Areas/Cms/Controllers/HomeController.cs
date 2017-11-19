using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
