using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Extensions;
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

        /// <summary>
        /// Get all websites
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var websites = websiteService.GetAllReadOnly();
                return Ok(websites);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get website by its numeric Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
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

        /// <summary>
        /// Gets website by its Vanity Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Inserts a new website
        /// </summary>
        /// <param name="websiteEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var returnValue = websiteService.Save(websiteEditModel);
                return CreatedAtRoute(nameof(Post), new { returnValue = returnValue }, websiteEditModel);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update a website
        /// </summary>
        /// <param name="id"></param>
        /// <param name="websiteEditModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var editModelToUpdate = websiteService.SetupEditModel(websiteEditModel.VanityId);

                websiteEditModel.MapTo(editModelToUpdate);

                var returnValue = websiteService.Save(editModelToUpdate);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a website
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var returnValue = websiteService.Delete(id);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}