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
        public async Task<IActionResult> Create(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Categories", PageType.Create, panelTitle: "Create a new category");
                return View("CreateEdit", categoryEditModel);
            }

            return await Save(categoryEditModel);
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

            var categoryToUpdate = await service.SetupCategoryEditModel(categoryEditModel.VanityId);

            if (await TryUpdateModelAsync((CategoryEditModel)categoryToUpdate))
            {
                return await Save(categoryEditModel);
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
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = service.GetCategoriesForDataTable(parameters);

            var dataTable = await service.BuildDataTable<Category>(items.Data, items.RecordsCount);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        private async Task<IActionResult> Save(CategoryEditModel categoryEditModel)
        {
            var returnValue = await service.SaveCategory(categoryEditModel);

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
