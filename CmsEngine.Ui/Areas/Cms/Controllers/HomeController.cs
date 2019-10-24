using System.Threading.Tasks;
using CmsEngine.Application.Services;
using CmsEngine.Ui.Areas.Cms.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Admin.Controllers
{
    [Area("Cms")]
    public class HomeController : BaseController
    {
        private readonly IEmailService _emailService;

        public HomeController(ILoggerFactory loggerFactory, IService service, IEmailService emailService)
                             : base(loggerFactory, service)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            SetupMessages("Dashboard");
            return View(await _emailService.GetOrderedByDate());
        }
    }
}
