using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Services;

namespace CmsEngine.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/website")]
    public class WebsiteController : Controller
    {
        private readonly WebsiteService websiteService;

        public WebsiteController(IUnitOfWork uow)
        {
            websiteService = new WebsiteService(uow);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var websites = websiteService.GetAllReadOnly();

            return websites.Select(x => x.Name);
        }

        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            var website = websiteService.GetByVanityId(id);
            return website.Name;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            var website = websiteService.GetById(id);
            return website.Name;
        }
    }
}