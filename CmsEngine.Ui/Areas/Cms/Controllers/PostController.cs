using System;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Helpers;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Core.Constants;
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

        public async Task<IActionResult> Create()
        {
            SetupMessages("Post", PageType.Create, panelTitle: "Create a new post");
            var postEditModel = await _postService.SetupEditModel();

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

            return await Save(postEditModel, nameof(PostController.Create));
        }

        public async Task<IActionResult> Edit(Guid vanityId)
        {
            SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
            var postEditModel = await _postService.SetupEditModel(vanityId);

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditModel postEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Posts", PageType.Edit, panelTitle: "Edit an existing post");
                TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
                return View("CreateEdit", postEditModel);
            }

            var postToUpdate = await _postService.SetupEditModel(postEditModel.VanityId);

            if (await TryUpdateModelAsync(postToUpdate))
            {
                return await Save(postEditModel, nameof(PostController.Edit));
            }

            TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
            return RedirectToAction(nameof(PostController.Edit), postEditModel);
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
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await PrepareAndUploadFiles(_env.WebRootPath, "Post");
        }

        private async Task<IActionResult> Save(PostEditModel postEditModel, string sender)
        {
            var returnValue = await _postService.Save(postEditModel);

            if (!returnValue.IsError)
            {
                TempData[MessageConstants.SuccessMessage] = returnValue.Message;
                return RedirectToAction(nameof(PostController.Index));
            }
            else
            {
                TempData[MessageConstants.DangerMessage] = returnValue.Message;
                return RedirectToAction(sender);
            }
        }
    }
}
