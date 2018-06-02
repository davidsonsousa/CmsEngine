using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
