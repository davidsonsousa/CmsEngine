using CmsEngine.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ErrorController");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(string code)
        {
            ErrorViewModel errorViewModel;

            switch (code)
            {
                case "404":
                    _logger.LogError("Page not found");
                    errorViewModel = new ErrorViewModel("404 - Page not found", "Sorry but this page does not exist");
                    break;
                default:
                    _logger.LogError("Default error");
                    errorViewModel = new ErrorViewModel("Something went wrong");
                    break;
            }

            ViewBag.ErrorCode = code;
            return View(errorViewModel);
        }
    }
}
