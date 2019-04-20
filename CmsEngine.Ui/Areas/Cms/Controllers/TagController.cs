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
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class TagController : BaseController
    {
        public TagController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                             ILogger<TagController> logger)
                      : base(uow, mapper, hca, userManager, logger) { }

        public IActionResult Index()
        {
            this.SetupMessages("Tags", PageType.List, panelTitle: "List of tags");
            //var tagViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Tag", PageType.Create, panelTitle: "Create a new tag");
            var tagEditModel = service.SetupTagEditModel();

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
            var tagEditModel = service.SetupTagEditModel(vanityId);

            return View("CreateEdit", tagEditModel);
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

            var tagToUpdate = (TagEditModel)service.SetupTagEditModel(tagEditModel.VanityId);

            if (await TryUpdateModelAsync(tagToUpdate))
            {
                return this.Save(tagEditModel);
            }

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(service.DeleteTag(vanityId));
        }

        [HttpPost("cms/tag/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(service.BulkDelete<Tag>(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataParameters parameters)
        {
            var filteredItems = service.FilterTag(parameters.Search.Value, service.GetAllTagsReadOnly<TagTableViewModel>());
            var orderedItems = service.OrderTag(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = service.BuildDataTable<Tag>(orderedItems, parameters.Start, parameters.Length);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        private IActionResult Save(TagEditModel tagEditModel)
        {
            var returnValue = service.SaveTag(tagEditModel);

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
    }
}
