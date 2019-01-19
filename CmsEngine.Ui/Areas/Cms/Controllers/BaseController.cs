using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Helpers;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly CmsService service;
        protected List<UploadFilesResult> fileList;
        protected readonly ILogger logger;

        public BaseController(ILogger logger)
        {
            this.logger = logger;
        }

        public BaseController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            service = new CmsService(uow, mapper, hca, userManager, logger);
            this.logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewBag.CurrentUser = service?.CurrentUser;
        }

        protected void SetupMessages(string pageTitle, PageType pageType, string description = "", string panelTitle = "")
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = description;
            ViewBag.PanelTitle = panelTitle;
            ViewBag.PageType = pageType.ToString();
        }

        protected void SetupMessages(string pageTitle, PageType pageType, string modelError, string generalError, string description = "", string panelTitle = "")
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = description;
            ViewBag.PanelTitle = panelTitle;
            ViewBag.PageType = pageType.ToString();

            if (!string.IsNullOrWhiteSpace(modelError))
            {
                ModelState.AddModelError("", modelError);
            }

            TempData["DangerMessage"] = generalError;
        }

        protected async Task<ContentResult> PrepareAndUploadFiles(string webrootPath, string folderName)
        {
            string folderPath = GetUploadFolderPath(webrootPath, folderName);

            fileList = new List<UploadFilesResult>();

            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length == 0)
                {
                    continue;
                }

                string originalFile = await UploadFile(folderPath, formFile);

                string fileSize = FileHelper.FormatFileSize(originalFile);
                string pathUrl = "";

                bool isImage = FileHelper.IsImage(formFile.FileName);
                if (isImage)
                {
                    var imageSizes = new List<(int Width, int Height)>
                    {
                        (120, 120),
                        (320, 213),
                        (640, 426)
                    };

                    foreach (var imageSize in imageSizes)
                    {
                        ResizeImages(folderPath, formFile, originalFile, imageSize.Width, imageSize.Height);
                    }

                    pathUrl = $"/image/{folderName}/";
                }
                else
                {
                    pathUrl = $"/file/{folderName}/";
                }

                fileList.Add(new UploadFilesResult
                {
                    FileName = formFile.FileName,
                    Path = pathUrl,
                    Length = formFile.Length,
                    ContentType = formFile.ContentType,
                    IsImage = isImage,
                    Size = fileSize
                });
            }
            return Content(JsonConvert.SerializeObject(fileList).ToLowerInvariant(), "application/json");
        }

        private void ResizeImages(string folderPath, IFormFile formFile, string originalFile, int width, int height)
        {
            string thumbnailFileName = Path.Combine(folderPath, $"{width}x{height}_{formFile.FileName}");
            FileHelper.ResizeImage(originalFile, thumbnailFileName, width, height, true);
        }

        private async Task<string> UploadFile(string folderPath, IFormFile formFile)
        {
            string filePath = Path.Combine(folderPath, Path.GetFileName(formFile.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return filePath;
        }

        private string GetUploadFolderPath(string webrootPath, string folderName)
        {
            string folder = Path.Combine(webrootPath, "UploadedFiles", folderName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

    }
}
