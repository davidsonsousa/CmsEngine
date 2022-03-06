namespace CmsEngine.Ui.Controllers;

public class HomeController : BaseController
{
    private readonly ICmsEngineEmailSender _emailSender;
    private readonly IPageService _pageService;
    private readonly IXmlService _xmlService;
    private readonly IEmailService _emailService;

    public HomeController(ILoggerFactory loggerFactory, ICmsEngineEmailSender emailSender, IPageService pageService, IXmlService xmlService,
                          ICategoryService categoryService, ITagService tagService, IService service, IPostService postService,
                          IEmailService emailService)
                         : base(loggerFactory, service, categoryService, pageService, postService, tagService)
    {
        _emailSender = emailSender;
        _pageService = pageService;
        _xmlService = xmlService;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        Instance.PageTitle = $"{Instance.Name}";
        return View(Instance);
    }

    public async Task<IActionResult> PageAsync(string slug)
    {
        Instance.SelectedDocument = await _pageService.GetBySlug(slug);

        if (Instance.SelectedDocument == null)
        {
            return NotFound();
        }

        Instance.PageTitle = $"{Instance.SelectedDocument.Title} - {Instance.Name}";
        return View(Instance);
    }

    public IActionResult Archive()
    {
        Instance.PageTitle = $"Archive - {Instance.Name}";
        return View(Instance);
    }

    public IActionResult Contact()
    {
        Instance.PageTitle = $"Contact - {Instance.Name}";
        return View(Instance);
    }

    [HttpPost]
    public async Task<IActionResult> ContactAsync(ContactForm contactForm, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
            return View(Instance);
        }

        ViewData["ReturnUrl"] = returnUrl;
        contactForm.To = Instance.ContactDetails.Email;

        try
        {
            if ((await _emailService.Save(contactForm)).IsError)
            {
                throw new Exception("Error when saving e-mail");
            }

            await _emailSender.SendEmailAsync(contactForm);
            TempData[MessageConstants.SuccessMessage] = "Your message was sent. I will answer as soon as I can.";
        }
        catch (EmailException)
        {
            TempData[MessageConstants.DangerMessage] = "We could not send the messsage. Please try other communication channels.";
        }

        return RedirectToAction(nameof(HomeController.Contact));
    }

    public async Task<IActionResult> SitemapAsync()
    {
        var sitemap = await _xmlService.GenerateSitemap();
        return Content(sitemap.ToString(), "text/xml");
    }
}
