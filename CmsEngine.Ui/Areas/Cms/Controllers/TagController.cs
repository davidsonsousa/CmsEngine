using System;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Helpers;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Core.Constants;
using CmsEngine.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ILoggerFactory loggerFactory, IService service, ITagService tagService)
                            : base(loggerFactory, service)
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
        public async Task<IActionResult> CreateAsync(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Tags", PageType.Create, panelTitle: "Create a new tag");
                return View("CreateEdit", tagEditModel);
            }

            return await SaveAsync(tagEditModel, nameof(TagController.Create));
        }

        public async Task<IActionResult> EditAsync(Guid vanityId)
        {
            SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
            var tagEditModel = await _tagService.SetupEditModel(vanityId);

            return View("CreateEdit", tagEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(TagEditModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Tags", PageType.Edit, panelTitle: "Edit an existing tag");
                return View("CreateEdit", tagEditModel);
            }

            var tagToUpdate = await _tagService.SetupEditModel(tagEditModel.VanityId);

            if (await TryUpdateModelAsync(tagToUpdate))
            {
                return await SaveAsync(tagEditModel, nameof(TagController.EditAsync));
            }
            TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
            return RedirectToAction(nameof(TagController.EditAsync), tagEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid vanityId)
        {
            return Ok(await _tagService.Delete(vanityId));
        }

        [HttpPost("cms/tag/bulk-delete")]
        public async Task<IActionResult> BulkDeleteAsync([FromForm]Guid[] vanityId)
        {
            return Ok(await _tagService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetDataAsync([FromForm]DataParameters parameters)
        {
            Guard.ThrownExceptionIfNull(parameters, nameof(parameters));

            var items = await _tagService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

            return Ok(dataTable);
        }

        private async Task<IActionResult> SaveAsync(TagEditModel tagEditModel, string sender)
        {
            var returnValue = await _tagService.Save(tagEditModel);

            if (!returnValue.IsError)
            {
                TempData[MessageConstants.SuccessMessage] = returnValue.Message;
                return RedirectToAction(nameof(TagController.Index));
            }
            else
            {
                TempData[MessageConstants.DangerMessage] = returnValue.Message;
                return RedirectToAction(sender);
            }
        }
    }
}
