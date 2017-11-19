using System;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class PostController : BaseController
    {
        private readonly PostService postService;

        public PostController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            postService = new PostService(uow, mapper, hca);
        }

        public IActionResult Index()
        {
            this.SetupMessages("Posts", PageType.List, panelTitle: "List of posts");
            //var postViewModel = postService.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Post", PageType.Create, panelTitle: "Create a new post");
            var postEditModel = postService.SetupEditModel();

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
            var postViewModel = postService.SetupEditModel(vanityId);

            return View("CreateEdit", postViewModel);
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

            var postToUpdate = (PostEditModel)postService.SetupEditModel(postEditModel.VanityId);

            if (await TryUpdateModelAsync(postToUpdate))
            {
                return this.Save(postEditModel);
            }

            return View("CreateEdit", postEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(postService.Delete(vanityId));
        }

        [HttpPost("cms/post/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(postService.BulkDelete(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataTableParameters parameters)
        {
            var filteredItems = postService.Filter(parameters.Search.Value, postService.GetAllReadOnly());
            var orderedItems = postService.Order(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = postService.BuildDataTable(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(PostEditModel postEditModel)
        {
            var returnValue = postService.Save(postEditModel);

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

        #endregion

    }
}
