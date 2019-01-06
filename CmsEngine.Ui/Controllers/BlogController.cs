using System.Linq;
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

        public IActionResult Index(int page = 1, string q = "")
        {
            return View(instance);
        }

        public IActionResult Post(string slug)
        {
            instance.SelectedDocument = (PostViewModel)service.GetPostBySlug(slug);
            instance.PageTitle = $"{instance.SelectedDocument.Title} - {instance.Name}";
            return View(instance);
        }

        public IActionResult Category(string slug, int page = 1)
        {
            instance.PagedPosts = service.GetPagedPostsByCategoryReadOnly<PostViewModel>(slug, page);
            var selectedCategory = instance.PagedPosts.SelectMany(p => p.Categories.Where(c => c.Slug == slug).Select(x => x.Name)).FirstOrDefault();
            instance.PageTitle = $"{selectedCategory} - {instance.Name}";
            return View("Index", instance);
        }

        public IActionResult Tag(string slug, int page = 1)
        {
            instance.PagedPosts = service.GetPagedPostsByTagReadOnly<PostViewModel>(slug, page);
            var selectedTag = instance.PagedPosts.SelectMany(p => p.Tags.Where(t => t.Slug == slug).Select(x => x.Name)).FirstOrDefault();
            instance.PageTitle = $"#{selectedTag} - {instance.Name}";
            return View("Index", instance);
        }

        public IActionResult Feed()
        {
            return Content(service.GenerateFeed().ToString(), "text/xml");
        }
    }
}
