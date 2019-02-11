using System.Globalization;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class BaseController : Controller
    {
        protected readonly CmsService service;
        protected readonly InstanceViewModel instance;
        protected readonly ILogger logger;

        public BaseController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            service = new CmsService(uow, mapper, hca, userManager, logger);
            instance = service.Instance;
            this.logger = logger;

            var cultureInfo = new CultureInfo(instance.Culture);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.ActionArguments.TryGetValue("q", out object searchValue))
            {
                instance.PagedPosts = service.GetPagedPostsFullTextSearch<PostViewModel>(DocumentStatus.Published, 1, searchValue.ToString());
            }
            else
            {
                if (context.ActionArguments.TryGetValue("page", out object value) && int.TryParse(value.ToString(), out int page))
                {
                    instance.PagedPosts = service.GetPagedPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published, page);
                }
                else
                {
                    instance.PagedPosts = service.GetPagedPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published);
                }
            }

            instance.LatestPosts = service.GetPostsByStatusReadOnly<PostViewModel>(DocumentStatus.Published, 3);
            instance.Pages = service.GetPagesByStatusReadOnly<PageViewModel>(DocumentStatus.Published);
            instance.Categories = service.GetCategoriesWithPostCount<CategoryViewModel>();
            instance.Tags = service.GetAllTagsReadOnly<TagViewModel>();
        }
    }
}
