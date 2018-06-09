using System;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class PostController : BaseController
    {
        public PostController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager) : base(uow, mapper, hca, userManager) { }

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
            var postViewModel = service.SetupPostEditModel(vanityId);

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
            var filteredItems = service.FilterPost(parameters.Search.Value, service.GetAllPostsReadOnly());
            var orderedItems = service.OrderPost(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = service.BuildDataTable<Post>(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

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

        #endregion

    }
}
