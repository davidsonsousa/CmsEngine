using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Ui.Areas.Cms.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                              ILogger<HomeController> logger)
                       : base(uow, mapper, hca, userManager, logger) { }

        public IActionResult Index()
        {
            this.SetupMessages("Dashboard");
            return View();
        }
    }
}
