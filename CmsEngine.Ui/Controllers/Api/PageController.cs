using System;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PageController : Controller
    {
        private readonly PageService pageService;

        public PageController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            pageService = new PageService(uow, mapper, hca);
        }

        /// <summary>
        /// Get all pages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var pages = pageService.GetAllReadOnly();
                return Ok(pages);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get page by its numeric Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var page = pageService.GetById(id);

                if (page == null)
                {
                    return NotFound();
                }

                return Ok(page);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all pages for the DataTable
        /// </summary>
        /// <returns></returns>
        [HttpGet("datatable")]
        public IActionResult GetDataTable()
        {
            try
            {
                var pages = pageService.GetAllReadOnly();
                var dataTable = pageService.BuildDataTable(pages);
                return Ok(dataTable);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets page by its Vanity Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var page = pageService.GetById(id);

                if (page == null)
                {
                    return NotFound();
                }

                return Ok(page);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Inserts a new page
        /// </summary>
        /// <param name="pageEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var returnValue = pageService.Save(pageEditModel);
                return CreatedAtRoute(null, returnValue);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update a page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageEditModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            pageEditModel.VanityId = id;

            try
            {
                var returnValue = pageService.Save(pageEditModel);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var returnValue = pageService.Delete(id);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
