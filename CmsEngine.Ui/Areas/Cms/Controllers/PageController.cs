using System;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Helpers;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Core.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class PageController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPageService _pageService;

        public PageController(ILoggerFactory loggerFactory, IService service,
                              IWebHostEnvironment env, IPageService pageService) : base(loggerFactory, service)
        {
            _env = env;
            _pageService = pageService;
        }

        public IActionResult Index()
        {
            SetupMessages("Pages", PageType.List, panelTitle: "List of pages");
            return View("List");
        }

        public IActionResult Create()
        {
            SetupMessages("Page", PageType.Create, panelTitle: "Create a new page");
            var pageEditModel = _pageService.SetupEditModel();

            return View("CreateEdit", pageEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Pages", PageType.Create, panelTitle: "Create a new page");
                return View("CreateEdit", pageEditModel);
            }

            return await Save(pageEditModel, nameof(PageController.Create));
        }

        public async Task<IActionResult> Edit(Guid vanityId)
        {
            SetupMessages("Pages", PageType.Edit, panelTitle: "Edit an existing page");
            var pageEditModel = await _pageService.SetupEditModel(vanityId);

            return View("CreateEdit", pageEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Pages", PageType.Edit, panelTitle: "Edit an existing page");
                TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
                return View("CreateEdit", pageEditModel);
            }

            var pageToUpdate = await _pageService.SetupEditModel(pageEditModel.VanityId);

            if (await TryUpdateModelAsync(pageToUpdate))
            {
                return await Save(pageEditModel, nameof(PageController.Edit));
            }

            TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
            return RedirectToAction(nameof(PageController.Edit), pageEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid vanityId)
        {
            return Ok(await _pageService.Delete(vanityId));
        }

        [HttpPost("cms/page/bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(await _pageService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = await _pageService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages()
        {
            return await UploadImage(_env.WebRootPath, "Page");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await PrepareAndUploadFiles(_env.WebRootPath, "Page");
        }

        private async Task<IActionResult> Save(PageEditModel pageEditModel, string sender)
        {
            var returnValue = await _pageService.Save(pageEditModel);

            if (!returnValue.IsError)
            {
                TempData[MessageConstants.SuccessMessage] = returnValue.Message;
                return RedirectToAction(nameof(PageController.Index));
            }
            else
            {
                TempData[MessageConstants.DangerMessage] = returnValue.Message;
                return RedirectToAction(sender);
            }
        }
    }
}
