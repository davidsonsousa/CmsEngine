using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Controllers
{
    public class BaseController : Controller
    {
        protected readonly CmsService service;

        public BaseController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager)
        {
            service = new CmsService(uow, mapper, hca, userManager);
        }
    }
}
