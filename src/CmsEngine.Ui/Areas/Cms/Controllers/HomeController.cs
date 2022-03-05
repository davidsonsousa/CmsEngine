namespace CmsEngine.Ui.Areas.Cms.Controllers;

[Area("Cms")]
public class HomeController : BaseController
{
    private readonly IEmailService _emailService;

    public HomeController(ILoggerFactory loggerFactory, IService service, IEmailService emailService)
                         : base(loggerFactory, service)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        SetupMessages("Dashboard");
        return View(await _emailService.GetOrderedByDate());
    }
}