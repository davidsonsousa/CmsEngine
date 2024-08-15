namespace CmsEngine.Ui.Areas.Cms.Controllers;

[Area("Cms")]
public class HomeController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IPageService _pageService;
    private readonly IPostService _postService;
    private readonly ITagService _tagService;

    public HomeController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService,
                          IPageService pageService, IPostService postService, ITagService tagService)
                         : base(loggerFactory, service)
    {
        _categoryService = categoryService;
        _pageService = pageService;
        _postService = postService;
        _tagService = tagService;

    }

    public async Task<IActionResult> IndexAsync()
    {
        SetupMessages("Dashboard");

        var homeViewModel = new HomeViewModel
        {
            CategoryCount = await _categoryService.GetCategoryCount(),
            PageCount = await _pageService.GetPageCount(),
            PostCount = await _postService.GetPostCount(),
            TagCount = await _tagService.GetTagCount()
        };

        return View(homeViewModel);
    }
}