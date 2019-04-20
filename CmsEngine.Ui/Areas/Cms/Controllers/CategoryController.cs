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
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                                  ILogger<CategoryController> logger)
                           : base(uow, mapper, hca, userManager, logger) { }

        public IActionResult Index()
        {
            this.SetupMessages("Categories", PageType.List, panelTitle: "List of categories");
            //var categoryViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Category", PageType.Create, panelTitle: "Create a new category");
            var categoryEditModel = service.SetupCategoryEditModel();

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
            var categoryEditModel = service.SetupCategoryEditModel(vanityId);

            return View("CreateEdit", categoryEditModel);
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

            var categoryToUpdate = (CategoryEditModel)service.SetupCategoryEditModel(categoryEditModel.VanityId);

            if (await TryUpdateModelAsync(categoryToUpdate))
            {
                return this.Save(categoryEditModel);
            }

            return View("CreateEdit", categoryEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(service.DeleteCategory(vanityId));
        }

        [HttpPost("cms/category/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(service.BulkDelete<Category>(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataParameters parameters)
        {
            var filteredItems = service.FilterCategory(parameters.Search.Value, service.GetAllCategoriesReadOnly<CategoryTableViewModel>());
            var orderedItems = service.OrderCategory(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = service.BuildDataTable<Category>(orderedItems, parameters.Start, parameters.Length);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        private IActionResult Save(CategoryEditModel categoryEditModel)
        {
            var returnValue = service.SaveCategory(categoryEditModel);

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
    }
}
