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
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService)
                                 : base(loggerFactory, service)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            SetupMessages("Categories", PageType.List, panelTitle: "List of categories");
            return View("List");
        }

        public IActionResult Create()
        {
            SetupMessages("Category", PageType.Create, panelTitle: "Create a new category");
            var categoryEditModel = _categoryService.SetupEditModel();

            return View("CreateEdit", categoryEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Categories", PageType.Create, panelTitle: "Create a new category");
                return View("CreateEdit", categoryEditModel);
            }

            return await Save(categoryEditModel, nameof(CategoryController.Create));
        }

        public async Task<IActionResult> Edit(Guid vanityId)
        {
            SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
            var categoryEditModel = await _categoryService.SetupEditModel(vanityId);

            return View("CreateEdit", categoryEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
                TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
                return View("CreateEdit", categoryEditModel);
            }

            var categoryToUpdate = await _categoryService.SetupEditModel(categoryEditModel.VanityId);

            if (await TryUpdateModelAsync(categoryToUpdate))
            {
                return await Save(categoryEditModel, nameof(CategoryController.Edit));
            }

            TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
            return RedirectToAction(nameof(CategoryController.Edit), categoryEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid vanityId)
        {
            return Ok(await _categoryService.Delete(vanityId));
        }

        [HttpPost("cms/category/bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(await _categoryService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            Guard.ThrownExceptionIfNull(parameters, nameof(parameters));

            var items = await _categoryService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

            return Ok(dataTable);
        }

        private async Task<IActionResult> Save(CategoryEditModel categoryEditModel, string sender)
        {
            var returnValue = await _categoryService.Save(categoryEditModel);

            if (!returnValue.IsError)
            {
                TempData[MessageConstants.SuccessMessage] = returnValue.Message;
                return RedirectToAction(nameof(CategoryController.Index));
            }
            else
            {
                TempData[MessageConstants.DangerMessage] = returnValue.Message;
                return RedirectToAction(sender);
            }
        }
    }
}
