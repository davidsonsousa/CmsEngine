using System;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class PostController : BaseController
    {
        private readonly IHostingEnvironment _env;

        public PostController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                              IHostingEnvironment env, ILogger<PostController> logger) : base(uow, mapper, hca, userManager, logger)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            this.SetupMessages("Posts", PageType.List, panelTitle: "List of posts");
            //var postViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Post", PageType.Create, panelTitle: "Create a new post");
            var postEditModel = service.SetupPostEditModel();

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Posts", PageType.Create, panelTitle: "Create a new post");
                return View("CreateEdit", postEditModel);
            }

            return this.Save(postEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
            var postEditModel = service.SetupPostEditModel(vanityId);

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
                return View("CreateEdit", postEditModel);
            }

            var postToUpdate = (PostEditModel)service.SetupPostEditModel(postEditModel.VanityId);

            if (await TryUpdateModelAsync(postToUpdate))
            {
                return this.Save(postEditModel);
            }

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(service.DeletePost(vanityId));
        }

        [HttpPost("cms/post/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(service.BulkDelete<Post>(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataParameters parameters)
        {
            var items = service.GetPostsForDataTable(parameters);

            var dataTable = service.BuildDataTable<Post>(items.Data, items.RecordsCount);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await this.PrepareAndUploadFiles(_env.WebRootPath, "Post");
        }

        private IActionResult Save(PostEditModel postEditModel)
        {
            var returnValue = service.SavePost(postEditModel);

            if (!returnValue.IsError)
            {
                TempData["SuccessMessage"] = returnValue.Message;
            }
            else
            {
                return View("CreateEdit", postEditModel);
            }

            return RedirectToAction("Index");
        }
    }
}
