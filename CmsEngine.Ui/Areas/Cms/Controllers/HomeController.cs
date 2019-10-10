using CmsEngine.Application.Services;
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
        public HomeController(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IService service)
                             : base(uow, hca, loggerFactory, service) { }

        public IActionResult Index()
        {
            SetupMessages("Dashboard");
            return View();
        }
    }
}
