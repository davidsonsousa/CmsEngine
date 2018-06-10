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
            return View(instance);
        }

        public IActionResult Post(string slug)
        {
            instance.SelectedPost = (PostViewModel)service.GetPostBySlug(slug);
            return View(instance);
        }
    }
}
