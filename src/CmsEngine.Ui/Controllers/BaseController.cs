namespace CmsEngine.Ui.Controllers;

public class BaseController : Controller
{
    public InstanceViewModel Instance { get; private set; }

    public ILogger Logger { get; private set; }

    private readonly ICategoryService _categoryService;
    private readonly IPageService _pageService;
    private readonly IPostService _postService;
    private readonly ITagService _tagService;

    public BaseController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService, IPageService pageService, IPostService postService, ITagService tagService)
    {
        Guard.Against.Null(loggerFactory);
        Guard.Against.Null(service);

        Logger = loggerFactory.CreateLogger("BaseController");
        Instance = service.Instance;

        _categoryService = categoryService;
        _pageService = pageService;
        _postService = postService;
        _tagService = tagService;

        var cultureInfo = new CultureInfo(Instance.Culture);

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Guard.Against.Null(context);

        if (context.ActionArguments.TryGetValue("q", out var searchValue))
        {
            // Showing searched posts
            Instance.PagedPosts = await _postService.FindPublishedForPaginationOrderByDateDescending(searchValue?.ToString() ?? string.Empty);
        }
        else
        {
            if (context.ActionArguments.TryGetValue("page", out var value) && value is not null && int.TryParse(value.ToString(), out var page))
            {
                // Showing posts after paging
                Instance.PagedPosts = await _postService.GetPublishedForPagination(page);
            }
            else
            {
                // Showing regular posts
                Instance.PagedPosts = await _postService.GetPublishedForPagination();
            }
        }

        Instance.LatestPosts = await _postService.GetPublishedLatestPosts(3);
        Instance.Pages = await _pageService.GetAllPublished();
        Instance.Categories = await _categoryService.GetCategoriesWithPostCount();
        Instance.CategoriesWithPosts = await _categoryService.GetCategoriesWithPost();
        Instance.Tags = await _tagService.GetAllTags();

        await base.OnActionExecutionAsync(context, next);
    }

    protected override void Dispose(bool disposing)
    {
        Instance.Categories = Array.Empty<CategoryViewModel>();
        Instance.CategoriesWithPosts = Array.Empty<CategoryViewModel>();
        Instance.LatestPosts = Array.Empty<PostViewModel>();
        Instance.Pages = Array.Empty<PageViewModel>();
        Instance.Tags = Array.Empty<TagViewModel>();
        Instance.PagedPosts.Clear();

        _categoryService.Dispose();
        _pageService.Dispose();
        _postService.Dispose();
        _tagService.Dispose();

        base.Dispose(disposing);
    }
}
