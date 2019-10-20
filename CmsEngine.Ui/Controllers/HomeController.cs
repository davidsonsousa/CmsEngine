using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CmsEngine.Application.Helpers.Email;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels;
using CmsEngine.Core.Constants;
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

        public HomeController(ILoggerFactory loggerFactory, IEmailSender emailSender, IPageService pageService, IXmlService xmlService,
                              ICategoryService categoryService, ITagService tagService, IService service, IPostService postService)
                             : base(loggerFactory, service, categoryService, pageService, postService, tagService)
        {
            _emailSender = emailSender;
            _pageService = pageService;
            _xmlService = xmlService;
        }

        public IActionResult Index()
        {
            instance.PageTitle = $"{instance.Name}";
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
            instance.PageTitle = $"Contact - {instance.Name}";
            return View(instance);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactForm contactForm, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
                return View(instance);
            }

            ViewData["ReturnUrl"] = returnUrl;
            contactForm.To = instance.ContactDetails.Email;

            try
            {
                await _emailSender.SendEmailAsync(contactForm);
                TempData[MessageConstants.SuccessMessage] = "Your message was sent. I will answer as soon as I can.";
            }
            catch (Exception ex)
            {
                TempData[MessageConstants.DangerMessage] = "We could not send the messsage. Please try other communication channels.";
            }

            return RedirectToAction(nameof(HomeController.Contact));
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
