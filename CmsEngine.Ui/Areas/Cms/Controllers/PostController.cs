using System;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Helpers;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class PostController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPostService _postService;

        public PostController(ILoggerFactory loggerFactory, IService service,
                              IWebHostEnvironment env, IPostService postService) : base(loggerFactory, service)
        {
            _env = env;
            _postService = postService;
        }

        public IActionResult Index()
        {
            SetupMessages("Posts", PageType.List, panelTitle: "List of posts");
            return View("List");
        }

        public IActionResult Create()
        {
            SetupMessages("Post", PageType.Create, panelTitle: "Create a new post");
            var postEditModel = _postService.SetupEditModel();

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Posts", PageType.Create, panelTitle: "Create a new post");
                return View("CreateEdit", postEditModel);
            }

            return await Save(postEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
            var postEditModel = _postService.SetupEditModel(vanityId);

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
                return View("CreateEdit", postEditModel);
            }

            var postToUpdate = await _postService.SetupEditModel(postEditModel.VanityId);

            if (await TryUpdateModelAsync(postToUpdate))
            {
                return await Save(postEditModel);
            }

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid vanityId)
        {
            return Ok(await _postService.Delete(vanityId));
        }

        [HttpPost("cms/post/bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(await _postService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = await _postService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw);

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await PrepareAndUploadFiles(_env.WebRootPath, "Post");
        }

        private async Task<IActionResult> Save(PostEditModel postEditModel)
        {
            var returnValue = await _postService.Save(postEditModel);

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
