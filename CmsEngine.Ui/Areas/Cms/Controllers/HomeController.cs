using CmsEngine.Application.Services;
using CmsEngine.Ui.Areas.Cms.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : BaseController
    {
        public HomeController(ILoggerFactory loggerFactory, IService service)
                             : base(loggerFactory, service) { }

        public IActionResult Index()
        {
            SetupMessages("Dashboard");
            return View();
        }
    }
}
