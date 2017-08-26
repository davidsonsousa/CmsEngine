using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace CmsEngine.Ui.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;

        public CategoryController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            categoryService = new CategoryService(uow, mapper, hca);
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var categories = categoryService.GetAllReadOnly();
                return Ok(categories);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get category by its numeric Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var category = categoryService.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets category by its Vanity Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var category = categoryService.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Inserts a new category
        /// </summary>
        /// <param name="categoryEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var returnValue = categoryService.Save(categoryEditModel);
                return CreatedAtRoute(null, returnValue);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryEditModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            categoryEditModel.VanityId = id;

            try
            {
                var returnValue = categoryService.Save(categoryEditModel);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var returnValue = categoryService.Delete(id);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}