using CmsEngine.Data.AccessLayer;
using CmsEngine.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Get()
        {
            try
            {
                var websites = websiteService.GetAllReadOnly();
                return Ok(websites.Select(x => x.Name));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var website = websiteService.GetById(id);

                if (website == null)
                {
                    return NotFound();
                }

                return Ok(website.Name);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var website = websiteService.GetByVanityId(id);

                if (website == null)
                {
                    return NotFound();
                }

                return Ok(website.Name);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}