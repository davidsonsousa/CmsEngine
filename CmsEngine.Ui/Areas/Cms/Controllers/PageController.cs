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
    public class PageController : BaseController
    {
        private readonly IHostingEnvironment _env;

        public PageController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager,
                              IHostingEnvironment env, ILogger<PageController> logger) : base(uow, mapper, hca, userManager, logger)
        {
            _env = env;
        }

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
        public async Task<IActionResult> Create(PageEditModel pageEditModel)
        {
            if (!ModelState.IsValid)
            {
                this.SetupMessages("Pages", PageType.Create, panelTitle: "Create a new page");
                return View("CreateEdit", pageEditModel);
            }

            return await Save(pageEditModel);
        }

        public IActionResult Edit(Guid vanityId)
        {
            this.SetupMessages("Pages", PageType.Edit, panelTitle: "Edit an existing page");
            var pageEditModel = service.SetupPageEditModel(vanityId);

            return View("CreateEdit", pageEditModel);
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

            var pageToUpdate = await service.SetupPageEditModel(pageEditModel.VanityId);

            if (await TryUpdateModelAsync((PageEditModel)pageToUpdate))
            {
                return await Save(pageEditModel);
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
        public async Task<IActionResult> GetData([FromForm]DataParameters parameters)
        {
            var items = service.GetPagesForDataTable(parameters);

            var dataTable = await service.BuildDataTable<Page>(items.Data, items.RecordsCount);
            dataTable.Draw = parameters.Draw;

            return Ok(dataTable);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            return await this.PrepareAndUploadFiles(_env.WebRootPath, "Page");
        }

        private async Task<IActionResult> Save(PageEditModel pageEditModel)
        {
            var returnValue = await service.SavePage(pageEditModel);

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
    }
}
