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
    public class PostController : Controller
    {
        private readonly PostService postService;

        public PostController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            postService = new PostService(uow, mapper, hca);
        }

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var posts = postService.GetAllReadOnly();
                return Ok(posts);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all posts for the DataTable
        /// </summary>
        /// <returns></returns>
        [HttpGet("datatable")]
        public IActionResult GetDataTable()
        {
            try
            {
                var posts = postService.GetAllReadOnly();
                var dataTable = postService.BuildDataTable(posts);
                return Ok(dataTable);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get post by its numeric Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var post = postService.GetById(id);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets post by its Vanity Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var post = postService.GetById(id);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Inserts a new post
        /// </summary>
        /// <param name="postEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var returnValue = postService.Save(postEditModel);
                return CreatedAtRoute(null, returnValue);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="postEditModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            postEditModel.VanityId = id;

            try
            {
                var returnValue = postService.Save(postEditModel);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var returnValue = postService.Delete(id);
                return Ok(returnValue);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
