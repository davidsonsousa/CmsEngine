using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Controllers
{
    public class BlogController : BaseController
    {
        public BlogController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager)
                       : base(uow, mapper, hca, userManager)
        {
        }

        public IActionResult Index(int page = 1)
        {
            return View(instance);
        }

        public IActionResult Post(string slug)
        {
            instance.SelectedPost = (PostViewModel)service.GetPostBySlug(slug);
            return View(instance);
        }

        public IActionResult Category(string slug, int page = 1)
        {
            instance.PagedPosts = service.GetPagedPostsByCategoryReadOnly<PostViewModel>(slug, page);
            return View("Index", instance);
        }

        public IActionResult Tag(string slug, int page = 1)
        {
            instance.PagedPosts = service.GetPagedPostsByTagReadOnly<PostViewModel>(slug, page);
            return View("Index", instance);
        }
    }
}
