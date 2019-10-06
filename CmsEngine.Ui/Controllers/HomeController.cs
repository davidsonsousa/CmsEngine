using System.Diagnostics;
using System.Threading.Tasks;
using CmsEngine.Application.Helpers.Email;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IEmailSender _emailSender;
        private readonly IPageService _pageService;
        private readonly IXmlService _xmlService;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, IPageService pageService, IXmlService xmlService)
                       : base(logger)
        {
            _emailSender = emailSender;
            _pageService = pageService;
            _xmlService = xmlService;
        }

        public IActionResult Index()
        {
            return View(instance);
        }

        public async Task<IActionResult> Page(string slug)
        {
            instance.SelectedDocument = await _pageService.GetBySlug(slug);

            if (instance.SelectedDocument == null)
            {
                return NotFound();
            }

            instance.PageTitle = $"{instance.SelectedDocument.Title} - {instance.Name}";
            return View(instance);
        }

        public IActionResult Archive()
        {
            instance.PageTitle = $"Archive - {instance.Name}";
            return View(instance);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            instance.PageTitle = $"Contact - {instance.Name}";
            return View(instance);
        }

        [HttpPost]
        public IActionResult Contact(ContactForm contactForm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            _emailSender.SendEmailAsync(contactForm);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public async Task<IActionResult> Sitemap()
        {
            var sitemap = await _xmlService.GenerateSitemap();
            return Content(sitemap.ToString(), "text/xml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            instance.PageTitle = $"Error - {instance.Name}";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
