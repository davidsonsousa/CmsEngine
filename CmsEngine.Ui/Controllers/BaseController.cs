using System.Globalization;
using System.Threading.Tasks;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class BaseController : Controller
    {
        //protected readonly IService service;
        protected readonly InstanceViewModel instance;
        protected readonly ILogger logger;

        private readonly ICategoryService _categoryService;
        private readonly IPageService _pageService;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;

        public BaseController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService, IPageService pageService, IPostService postService, ITagService tagService)
        {
            instance = service.Instance;
            logger = loggerFactory.CreateLogger("BaseController");

            _categoryService = categoryService;
            _pageService = pageService;
            _postService = postService;
            _tagService = tagService;

            var cultureInfo = new CultureInfo(instance.Culture);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("q", out object searchValue))
            {
                // Showing searched posts
                instance.PagedPosts = await _postService.FindPublishedForPaginationOrderByDateDescending(searchValue.ToString());
            }
            else
            {
                if (context.ActionArguments.TryGetValue("page", out object value) && int.TryParse(value.ToString(), out int page))
                {
                    // Showing posts after paging
                    instance.PagedPosts = await _postService.GetPublishedForPagination(page);
                }
                else
                {
                    // Showing regular posts
                    instance.PagedPosts = await _postService.GetPublishedForPagination();
                }
            }

            instance.LatestPosts = await _postService.GetPublishedLatestPosts(3);
            instance.Pages = await _pageService.GetAllPublished();
            instance.Categories = await _categoryService.GetCategoriesWithPostCount();
            instance.Tags = await _tagService.GetAllTags();


            await base.OnActionExecutionAsync(context, next);
        }
    }
}
