namespace CmsEngine.Ui.Controllers;

public class HomeController : BaseController
{
    private readonly IPageService _pageService;
    private readonly IXmlService _xmlService;

    public HomeController(ILoggerFactory loggerFactory, IPageService pageService, IXmlService xmlService,
                          ICategoryService categoryService, ITagService tagService, IService service, IPostService postService)
                         : base(loggerFactory, service, categoryService, pageService, postService, tagService)
    {
        _pageService = pageService;
        _xmlService = xmlService;
    }

    public IActionResult Index()
    {
        Instance.PageTitle = $"{Instance.Name}";
        Instance.CanonicalType = CanonicalType.Index;

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
        Instance.CanonicalType = CanonicalType.Page;

        return View(Instance);
    }

    public IActionResult Archive()
    {
        Instance.PageTitle = $"Archive - {Instance.Name}";
        Instance.CanonicalType = CanonicalType.Archive;

        return View(Instance);
    }

    public async Task<IActionResult> SitemapAsync()
    {
        var sitemap = await _xmlService.GenerateSitemap();
        return Content(sitemap.ToString(), "text/xml");
    }
}
