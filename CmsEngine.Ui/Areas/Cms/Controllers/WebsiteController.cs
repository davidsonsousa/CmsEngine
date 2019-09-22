using System;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class WebsiteController : BaseController
    {
        private readonly IHostingEnvironment _env;

        public WebsiteController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                                 IHostingEnvironment env, ILogger<WebsiteController> logger) : base(uow, mapper, hca, userManager, logger)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            this.SetupMessages("Websites", PageType.List, panelTitle: "List of websites");
            //var websiteViewModel = service.SetupViewModel();
            return View("List");
        }

        public IActionResult Create()
        {
            this.SetupMessages("Website", PageType.Create, panelTitle: "Create a new website");
            var websiteEditModel = service.SetupWebsiteEditModel();

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Websites", PageType.Create, panelTitle: "Create a new website");
                return View("CreateEdit", websiteEditModel);
            }

            return await Save(websiteEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Websites", PageType.Edit, panelTitle: "Edit an existing website");
            var websiteEditModel = service.SetupWebsiteEditModel(vanityId);

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WebsiteEditModel websiteEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Websites", PageType.Edit, panelTitle: "Edit an existing website");
                return View("CreateEdit", websiteEditModel);
            }

            var websiteToUpdate = await service.SetupWebsiteEditModel(websiteEditModel.VanityId);

            if (await TryUpdateModelAsync((WebsiteEditModel)websiteToUpdate))
            {
                return await Save(websiteEditModel);
            }

            return View("CreateEdit", websiteEditModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid vanityId)
        {
            return Ok(service.DeleteWebsite(vanityId));
        }

        [HttpPost("cms/website/bulk-delete")]
        public IActionResult BulkDelete([FromForm]Guid[] vanityId)
        {
            return Ok(service.BulkDelete<Website>(vanityId));
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = service.GetWebsitesForDataTable(parameters);

            var dataTable = await service.BuildDataTable<Website>(items.Data, items.RecordsCount);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await this.PrepareAndUploadFiles(_env.WebRootPath, "Website");
        }

        private async Task<IActionResult> Save(WebsiteEditModel websiteEditModel)
        {
            var returnValue = await service.SaveWebsite(websiteEditModel);

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
    }
}
