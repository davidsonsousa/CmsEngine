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
using Newtonsoft.Json;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly CmsService service;
        protected List<UploadFilesResult> fileList;

        public BaseController()
        {

        }

        public BaseController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager)
        {
            service = new CmsService(uow, mapper, hca, userManager);
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

        protected async Task<ContentResult> UploadFiles(string webrootPath, string folderName)
        {
            string folder = Path.Combine(webrootPath, "UploadedFiles", folderName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            fileList = new List<UploadFilesResult>();

            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length == 0)
                {
                    continue;
                }

                using (var stream = new FileStream(Path.Combine(folder, formFile.FileName), FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                bool isImage = FileHelper.IsImage(formFile.FileName);

                string savedFileName = Path.Combine(folder, Path.GetFileName(formFile.FileName));

                string fileSize = FileHelper.FormatFileSize(savedFileName);
                string pathUrl = "";

                if (isImage)
                {
                    // Generate the thumbnails for the images
                    string thumbnailFileName = Path.Combine(folder, "tn_" + formFile.FileName);
                    FileHelper.ResizeImage(savedFileName, thumbnailFileName, 316, 198, true);

                    pathUrl = $"/image/{folderName}/";
                }
                else
                {
                    pathUrl = $"/file/{folderName}/";
                }

                fileList.Add(new UploadFilesResult()
                {
                    FileName = formFile.FileName,
                    ThumbnailName = "tn_" + formFile.FileName,
                    Path = pathUrl,
                    Length = formFile.Length,
                    ContentType = formFile.ContentType,
                    IsImage = isImage,
                    Size = fileSize
                });
            }
            return Content(JsonConvert.SerializeObject(fileList).ToLowerInvariant(), "application/json");
        }
    }
}
