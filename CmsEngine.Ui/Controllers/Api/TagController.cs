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
    public class TagController : Controller
    {
        private readonly TagService tagService;

        public TagController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            tagService = new TagService(uow, mapper, hca);
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tags = tagService.GetAllReadOnly();
                return Ok(tags);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get tag by its numeric Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var tag = tagService.GetById(id);

                if (tag == null)
                {
                    return NotFound();
                }

                return Ok(tag);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all tags for the DataTable
        /// </summary>
        /// <returns></returns>
        [HttpGet("datatable")]
        public IActionResult GetDataTable()
        {
            try
            {
                var tags = tagService.GetAllReadOnly();
                var dataTable = tagService.BuildDataTable(tags);
                return Ok(dataTable);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets tag by its Vanity Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var tag = tagService.GetById(id);

                if (tag == null)
                {
                    return NotFound();
                }

                return Ok(tag);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Inserts a new tag
        /// </summary>
        /// <param name="tagEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var returnValue = tagService.Save(tagEditModel);
                return CreatedAtRoute(null, returnValue);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update a tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tagEditModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            tagEditModel.VanityId = id;

            try
            {
                var returnValue = tagService.Save(tagEditModel);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var returnValue = tagService.Delete(id);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
