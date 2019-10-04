using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Domain.Helpers;
using CmsEngine.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly Service service;
        protected List<UploadFilesResult> fileList;
        protected readonly ILogger logger;

        public BaseController(ILogger logger)
        {
            this.logger = logger;
        }

        public BaseController(IUnitOfWork uow, IHttpContextAccessor hca, ILogger logger)
        {
            service = new Service(uow, hca, logger);
            this.logger = logger;

            var cultureInfo = new CultureInfo(service.Instance.Culture);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewBag.CurrentUser = service?.CurrentUser;
        }

        protected void SetupMessages(string pageTitle)
        {
            ViewBag.PageTitle = pageTitle;
        }

        protected void SetupMessages(string pageTitle, PageType pageType, string description = "", string panelTitle = "")
        {
            this.SetupMessages(pageTitle);

            ViewBag.PageType = pageType.ToString();
            ViewBag.PageDescription = description;
            ViewBag.PanelTitle = panelTitle;
        }

        protected void SetupMessages(string pageTitle, PageType pageType, string modelError, string generalError, string description = "", string panelTitle = "")
        {
            this.SetupMessages(pageTitle, pageType, description, panelTitle);

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
