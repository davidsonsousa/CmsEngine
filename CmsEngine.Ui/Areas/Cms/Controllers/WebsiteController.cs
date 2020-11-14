using System;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Helpers;
using CmsEngine.Application.Services;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Core.Constants;
using CmsEngine.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class WebsiteController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IWebsiteService _websiteService;

        public WebsiteController(ILoggerFactory loggerFactory, IService service,
                                 IWebHostEnvironment env, IWebsiteService websiteService) : base(loggerFactory, service)
        {
            _env = env;
            _websiteService = websiteService;
        }

        public IActionResult Index()
        {
            SetupMessages("Websites", PageType.List, panelTitle: "List of websites");
            return View("List");
        }

        public IActionResult Create()
        {
            SetupMessages("Website", PageType.Create, panelTitle: "Create a new website");
            var websiteEditModel = _websiteService.SetupEditModel();

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Websites", PageType.Create, panelTitle: "Create a new website");
                return View("CreateEdit", websiteEditModel);
            }

            return await Save(websiteEditModel, nameof(WebsiteController.Create));
        }

        public async Task<IActionResult> Edit(Guid vanityId)
        {
            SetupMessages("Websites", PageType.Edit, panelTitle: "Edit an existing website");
            var websiteEditModel = await _websiteService.SetupEditModel(vanityId);

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                SetupMessages("Websites", PageType.Edit, panelTitle: "Edit an existing website");
                return View("CreateEdit", websiteEditModel);
            }

            var websiteToUpdate = await _websiteService.SetupEditModel(websiteEditModel.VanityId);

            if (await TryUpdateModelAsync(websiteToUpdate))
            {
                return await Save(websiteEditModel, nameof(WebsiteController.Edit));
            }
            TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
            return RedirectToAction(nameof(WebsiteController.Edit), websiteEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid vanityId)
        {
            return Ok(await _websiteService.Delete(vanityId));
        }

        [HttpPost("cms/website/bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(await _websiteService.DeleteRange(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            Guard.ThrownExceptionIfNull(parameters, nameof(parameters));

            var items = await _websiteService.GetForDataTable(parameters);
            var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages()
        {
            return await UploadImage(_env.WebRootPath, "Website");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await PrepareAndUploadFiles(_env.WebRootPath, "Website");
        }

        private async Task<IActionResult> Save(WebsiteEditModel websiteEditModel, string sender)
        {
            var returnValue = await _websiteService.Save(websiteEditModel);

            if (!returnValue.IsError)
            {
                TempData[MessageConstants.SuccessMessage] = returnValue.Message;
                return RedirectToAction(nameof(WebsiteController.Index));
            }
            else
            {
                TempData[MessageConstants.DangerMessage] = returnValue.Message;
                return RedirectToAction(sender);
            }
        }
    }
}
