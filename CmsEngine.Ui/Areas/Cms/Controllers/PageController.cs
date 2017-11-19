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
    public class PageController : BaseController
    {
        private readonly PageService pageService;

        public PageController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            pageService = new PageService(uow, mapper, hca);
        }

        public IActionResult Index()
        {
            this.SetupMessages("Pages", PageType.List, panelTitle: "List of pages");
            //var pageViewModel = pageService.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Page", PageType.Create, panelTitle: "Create a new page");
            var pageEditModel = pageService.SetupEditModel();

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
            var pageViewModel = pageService.SetupEditModel(vanityId);

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

            var pageToUpdate = (PageEditModel)pageService.SetupEditModel(pageEditModel.VanityId);

            if (await TryUpdateModelAsync(pageToUpdate))
            {
                return this.Save(pageEditModel);
            }

            return View("CreateEdit", pageEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(pageService.Delete(vanityId));
        }

        [HttpPost("cms/page/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(pageService.BulkDelete(vanityId));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataTableParameters parameters)
        {
            var filteredItems = pageService.Filter(parameters.Search.Value, pageService.GetAllReadOnly());
            var orderedItems = pageService.Order(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = pageService.BuildDataTable(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(PageEditModel pageEditModel)
        {
            var returnValue = pageService.Save(pageEditModel);

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
