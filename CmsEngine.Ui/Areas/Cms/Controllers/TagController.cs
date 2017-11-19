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
    public class TagController : BaseController
    {
        private readonly TagService tagService;

        public TagController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            tagService = new TagService(uow, mapper, hca);
        }

        public IActionResult Index()
        {
            this.SetupMessages("Tags", PageType.List, panelTitle: "List of tags");
            //var tagViewModel = tagService.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Tag", PageType.Create, panelTitle: "Create a new tag");
            var tagEditModel = tagService.SetupEditModel();

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Tags", PageType.Create, panelTitle: "Create a new tag");
                return View("CreateEdit", tagEditModel);
            }

            return this.Save(tagEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
            var tagViewModel = tagService.SetupEditModel(vanityId);

            return View("CreateEdit", tagViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
                return View("CreateEdit", tagEditModel);
            }

            var tagToUpdate = (TagEditModel)tagService.SetupEditModel(tagEditModel.VanityId);

            if (await TryUpdateModelAsync(tagToUpdate))
            {
                return this.Save(tagEditModel);
            }

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(tagService.Delete(vanityId));
        }

        [HttpPost("cms/tag/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(tagService.BulkDelete(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataTableParameters parameters)
        {
            var filteredItems = tagService.Filter(parameters.Search.Value, tagService.GetAllReadOnly());
            var orderedItems = tagService.Order(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = tagService.BuildDataTable(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(TagEditModel tagEditModel)
        {
            var returnValue = tagService.Save(tagEditModel);

            if (!returnValue.IsError)
            {
                TempData["SuccessMessage"] = returnValue.Message;
            }
            else
            {
                return View("CreateEdit", tagEditModel);
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
