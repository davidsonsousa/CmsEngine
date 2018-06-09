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
    public class PageController : BaseController
    {
        public PageController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager) : base(uow, mapper, hca, userManager) { }

        public IActionResult Index()
        {
            this.SetupMessages("Pages", PageType.List, panelTitle: "List of pages");
            //var pageViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Page", PageType.Create, panelTitle: "Create a new page");
            var pageEditModel = service.SetupPageEditModel();

            return View("CreateEdit", pageEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Pages", PageType.Create, panelTitle: "Create a new page");
                return View("CreateEdit", pageEditModel);
            }

            return this.Save(pageEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Pages", PageType.Edit, panelTitle: "Edit an existing page");
            var pageViewModel = service.SetupPageEditModel(vanityId);

            return View("CreateEdit", pageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Pages", PageType.Edit, panelTitle: "Edit an existing page");
                return View("CreateEdit", pageEditModel);
            }

            var pageToUpdate = (PageEditModel)service.SetupPageEditModel(pageEditModel.VanityId);

            if (await TryUpdateModelAsync(pageToUpdate))
            {
                return this.Save(pageEditModel);
            }

            return View("CreateEdit", pageEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(service.DeletePage(vanityId));
        }

        [HttpPost("cms/page/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(service.BulkDelete<Page>(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataParameters parameters)
        {
            var filteredItems = service.FilterPage(parameters.Search.Value, service.GetAllPagesReadOnly());
            var orderedItems = service.OrderPage(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = service.BuildDataTable<Page>(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(PageEditModel pageEditModel)
        {
            var returnValue = service.SavePage(pageEditModel);

            if (!returnValue.IsError)
            {
                TempData["SuccessMessage"] = returnValue.Message;
            }
            else
            {
                return View("CreateEdit", pageEditModel);
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
