using CmsEngine.Data;
using CmsEngine.Ui.Areas.Cms.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow, IHttpContextAccessor hca, ILogger<HomeController> logger) : base(uow, hca, logger) { }

        public IActionResult Index()
        {
            this.SetupMessages("Dashboard");
            return View();
        }
    }
}
