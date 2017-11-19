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
    public class CategoryController : BaseController
    {
        private readonly CategoryService categoryService;

        public CategoryController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            categoryService = new CategoryService(uow, mapper, hca);
        }

        public IActionResult Index()
        {
            this.SetupMessages("Categories", PageType.List, panelTitle: "List of categories");
            //var categoryViewModel = categoryService.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Category", PageType.Create, panelTitle: "Create a new category");
            var categoryEditModel = categoryService.SetupEditModel();

            return View("CreateEdit", categoryEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Categories", PageType.Create, panelTitle: "Create a new category");
                return View("CreateEdit", categoryEditModel);
            }

            return this.Save(categoryEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
            var categoryViewModel = categoryService.SetupEditModel(vanityId);

            return View("CreateEdit", categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
                return View("CreateEdit", categoryEditModel);
            }

            var categoryToUpdate = (CategoryEditModel)categoryService.SetupEditModel(categoryEditModel.VanityId);

            if (await TryUpdateModelAsync(categoryToUpdate))
            {
                return this.Save(categoryEditModel);
            }

            return View("CreateEdit", categoryEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(categoryService.Delete(vanityId));
        }

        [HttpPost("cms/category/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(categoryService.BulkDelete(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataTableParameters parameters)
        {
            var filteredItems = categoryService.Filter(parameters.Search.Value, categoryService.GetAllReadOnly());
            var orderedItems = categoryService.Order(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = categoryService.BuildDataTable(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(CategoryEditModel categoryEditModel)
        {
            var returnValue = categoryService.Save(categoryEditModel);

            if (!returnValue.IsError)
            {
                TempData["SuccessMessage"] = returnValue.Message;
            }
            else
            {
                return View("CreateEdit", categoryEditModel);
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
