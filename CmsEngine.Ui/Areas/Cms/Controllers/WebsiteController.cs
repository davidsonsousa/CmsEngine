using System;
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
    public class WebsiteController : BaseController
    {
        private readonly WebsiteService websiteService;

        public WebsiteController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            websiteService = new WebsiteService(uow, mapper, hca);
        }

        public IActionResult Index()
        {
            this.SetupMessages("Websites", panelTitle: "List of websites");
            //var websiteViewModel = websiteService.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Website", panelTitle: "Create a new website");
            var websiteEditModel = websiteService.SetupEditModel();

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Websites", panelTitle: "Create a new website");
                return View("CreateEdit", websiteEditModel);
            }

            return this.Save(websiteEditModel);
        }

        public IActionResult Edit(Guid id)
        {
            this.SetupMessages("Websites", panelTitle: "Edit an existing website");
            var websiteViewModel = websiteService.SetupEditModel(id);

            return View("CreateEdit", websiteViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(WebsiteEditModel websiteEditModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        this.SetupMessages("Websites", panelTitle: "Edit an existing website");
        //        return View("CreateEdit", websiteEditModel);
        //    }

        //    var websiteToUpdate = websiteService.SetupEditModel(websiteEditModel.Id);

        //    var isUpdated = await TryUpdateModelAsync(websiteToUpdate, "Item");

        //    if (isUpdated)
        //    {
        //        websiteEditModel = websiteToUpdate;
        //        return this.Save(websiteEditModel);
        //    }

        //    return View("CreateEdit", websiteEditModel);
        //}

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            return Ok(websiteService.Delete(id));
        }

        [HttpPost("cms/website/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] id)
        {
            return Ok(websiteService.BulkDelete(id));
        }

        [HttpPost]
        public IActionResult GetData([FromForm]DataTableParameters parameters)
        {
            var filteredItems = websiteService.Filter(parameters.Search.Value, websiteService.GetAllReadOnly());
            var orderedItems = websiteService.Order(parameters.Order[0].Column, parameters.Order[0].Dir, filteredItems);

            var dataTable = websiteService.BuildDataTable(orderedItems);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        #region Helpers

        private IActionResult Save(WebsiteEditModel websiteEditModel)
        {
            var returnValue = websiteService.Save(websiteEditModel);

            if (!returnValue.IsError)
            {
                TempData["SuccessMessage"] = returnValue.Message;
            }
            else
            {
                return View("CreateEdit", websiteEditModel);
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
