using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost]
        public IActionResult Post([FromBody]WebsiteEditModel websiteViewModel)
        {
            try
            {
                var returnValue = websiteService.Save(websiteViewModel);
                return CreatedAtRoute("DefaultApi", new { returnValue = returnValue }, websiteViewModel);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]WebsiteEditModel websiteViewModel)
        {
            try
            {
                websiteService.Save(websiteViewModel);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                websiteService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}