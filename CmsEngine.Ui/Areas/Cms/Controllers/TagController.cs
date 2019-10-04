using System;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.Helpers;
using CmsEngine.Domain.Services;
using CmsEngine.Domain.ViewModels.DataTableViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(IUnitOfWork uow, IHttpContextAccessor hca, ILogger<TagController> logger, ITagService tagService)
                            : base(uow, hca, logger)
        {
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            SetupMessages("Tags", PageType.List, panelTitle: "List of tags");
            //var tagViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            SetupMessages("Tag", PageType.Create, panelTitle: "Create a new tag");
            var tagEditModel = _tagService.SetupEditModel();

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Tags", PageType.Create, panelTitle: "Create a new tag");
                return View("CreateEdit", tagEditModel);
            }

            return await Save(tagEditModel);
        }

        public async Task<IActionResult> Edit(Guid vanityId)
        {
            SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
            var tagEditModel = await _tagService.SetupEditModel(vanityId);

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
                return View("CreateEdit", tagEditModel);
            }

            var tagToUpdate = await _tagService.SetupEditModel(tagEditModel.VanityId);

            if (await TryUpdateModelAsync(tagToUpdate))
            {
                return await Save(tagEditModel);
            }

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid vanityId)
        {
            return Ok(await _tagService.Delete(vanityId));
        }

        [HttpPost("cms/tag/bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(await _tagService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = await _tagService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw);

            return Ok(dataTable);
        }

        private async Task<IActionResult> Save(TagEditModel tagEditModel)
        {
            var returnValue = await _tagService.Save(tagEditModel);

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
