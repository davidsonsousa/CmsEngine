using System;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Angular.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WebsiteController : Controller
    {
        private readonly WebsiteService websiteService;

        public WebsiteController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            websiteService = new WebsiteService(uow, mapper, hca);
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
        /// Get all websites for the DataTable
        /// </summary>
        /// <returns></returns>
        [HttpGet("datatable")]
        public IActionResult GetDataTable()
        {
            try
            {
                var websites = websiteService.GetAllReadOnly();
                var dataTable = websiteService.BuildDataTable(websites);
                return Ok(dataTable);
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

                return Ok(website);
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

                return Ok(website);
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
                return CreatedAtRoute(null, returnValue);
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

            websiteEditModel.VanityId = id;

            try
            {
                var returnValue = websiteService.Save(websiteEditModel);
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
