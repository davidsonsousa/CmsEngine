using CmsEngine.Ui.Areas.Cms.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
