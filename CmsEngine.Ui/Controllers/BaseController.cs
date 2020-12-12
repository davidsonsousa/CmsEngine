using System.Globalization;
using System.Threading.Tasks;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels;
using CmsEngine.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class BaseController : Controller
    {
        //protected readonly IService service;
        public InstanceViewModel Instance { get; private set; }
        public ILogger Logger { get; private set; }

        private readonly ICategoryService _categoryService;
        private readonly IPageService _pageService;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;

        public BaseController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService, IPageService pageService, IPostService postService, ITagService tagService)
        {
            Guard.ThrownExceptionIfNull(loggerFactory, nameof(loggerFactory));
            Guard.ThrownExceptionIfNull(service, nameof(service));

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

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guard.ThrownExceptionIfNull(context, nameof(context));

            if (context.ActionArguments.TryGetValue("q", out object searchValue))
            {
                // Showing searched posts
                Instance.PagedPosts = await _postService.FindPublishedForPaginationOrderByDateDescending(searchValue.ToString());
            }
            else
            {
                if (context.ActionArguments.TryGetValue("page", out object value) && int.TryParse(value.ToString(), out int page))
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
    }
}
