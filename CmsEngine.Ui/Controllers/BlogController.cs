using System.Collections.Generic;
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

        public IActionResult Index()
        {
            var blogViewModel = new InstanceViewModel
            {
                PagedPosts = (IEnumerable<PostViewModel>)service.GetAllPostsReadOnly(4),
                LatestPosts = (IEnumerable<PostViewModel>)service.GetAllPostsReadOnly(3),
                Pages = (IEnumerable<PageViewModel>)service.GetAllPagesReadOnly(),
                Categories = (IEnumerable<CategoryViewModel>)service.GetAllCategoriesReadOnly(),
                Tags = (IEnumerable<TagViewModel>)service.GetAllTagsReadOnly()
            };

            return View(blogViewModel);
        }
    }
}
