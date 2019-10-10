using System.Globalization;
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

        private readonly IPostService _postService;

        public BaseController(ILoggerFactory loggerFactory, IService service)
        {
            instance = service.Instance;
            logger = loggerFactory.CreateLogger("BaseController");

            var cultureInfo = new CultureInfo(instance.Culture);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //base.OnActionExecuting(context);

            //if (context.ActionArguments.TryGetValue("q", out object searchValue))
            //{
            //    // Showing searched posts
            //    instance.PagedPosts = _postService.FindPublishedForPaginationOrderByDateDescending(searchValue.ToString());
            //}
            //else
            //{
            //    if (context.ActionArguments.TryGetValue("page", out object value) && int.TryParse(value.ToString(), out int page))
            //    {
            //        // Showing posts after paging
            //        instance.PagedPosts = service.GetPagedPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published, page);
            //    }
            //    else
            //    {
            //        // Showing regular posts
            //        instance.PagedPosts = service.GetPagedPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published);
            //    }
            //}

            //instance.LatestPosts = service.GetPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published, 3);
            //instance.Pages = service.GetPagesByStatusReadOnly<PageViewModel>(DocumentStatus.Published);
            //instance.Categories = service.GetCategoriesWithPosts<CategoryViewModel>();
            //instance.Tags = service.GetAllTagsReadOnly<TagViewModel>();
        }
    }
}
