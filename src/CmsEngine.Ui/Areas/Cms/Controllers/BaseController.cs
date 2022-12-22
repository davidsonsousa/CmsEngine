using System.Runtime.Versioning;

namespace CmsEngine.Ui.Areas.Cms.Controllers;

[Authorize]
public class BaseController : Controller
{
    public IService Service { get; private set; }
    public ILogger Logger { get; private set; }

    public BaseController(ILoggerFactory loggerFactory, IService service)
    {
        Guard.Against.Null(loggerFactory);
        Guard.Against.Null(service);

        Logger = loggerFactory.CreateLogger("CmsBaseController");
        Service = service;

        var cultureInfo = new CultureInfo(service.Instance.Culture);

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        ViewBag.CurrentUser = Service?.CurrentUser;
    }

    protected void SetupMessages(string pageTitle)
    {
        ViewBag.PageTitle = pageTitle;
    }

    protected void SetupMessages(string pageTitle, PageType pageType, string description = "", string panelTitle = "")
    {
        SetupMessages(pageTitle);

        ViewBag.PageType = pageType.ToString();
        ViewBag.PageDescription = description;
        ViewBag.PanelTitle = panelTitle;
    }

    protected void SetupMessages(string pageTitle, PageType pageType, string modelError, string generalError, string description = "", string panelTitle = "")
    {
        SetupMessages(pageTitle, pageType, description, panelTitle);

        if (!string.IsNullOrWhiteSpace(modelError))
        {
            ModelState.AddModelError("", modelError);
        }

        TempData[MessageConstants.DangerMessage] = generalError;
    }

    protected async Task<ContentResult> UploadImageAsync(string webrootPath, string folderName)
    {
        var folderPath = GetUploadFolderPath(webrootPath, folderName);

        var formFile = Request.Form.Files[0];

        if (formFile.Length == 0)
        {
            return Content(string.Empty);
        }

        _ = await UploadFileAsync(folderPath, formFile);

        var pathUrl = $"/image/{folderName}/";

        var returnImage = new TinyMceUploadResult
        {
            Location = $"{pathUrl}{formFile.FileName}"
        };

        return Content(JsonConvert.SerializeObject(returnImage).ToLowerInvariant(), "application/json");
    }

    [SupportedOSPlatform("windows")]
    protected async Task<ContentResult> PrepareAndUploadFilesAsync(string webrootPath, string folderName)
    {
        var folderPath = GetUploadFolderPath(webrootPath, folderName);

        var fileList = new List<UploadFilesResult>();

        foreach (var formFile in Request.Form.Files)
        {
            if (formFile.Length == 0)
            {
                continue;
            }

            var originalFile = await UploadFileAsync(folderPath, formFile);

            var fileSize = FileHelper.FormatFileSize(originalFile);
            var isImage = FileHelper.IsImage(formFile.FileName);
            var pathUrl = string.Empty;

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

    [SupportedOSPlatform("windows")]
    private static void ResizeImages(string folderPath, IFormFile formFile, string originalFile, int width, int height)
    {
        var thumbnailFileName = Path.Combine(folderPath, $"{width}x{height}_{formFile.FileName}");
        FileHelper.ResizeImage(originalFile, thumbnailFileName, width, height, true);
    }

    private async static Task<string> UploadFileAsync(string folderPath, IFormFile formFile)
    {
        var filePath = Path.Combine(folderPath, Path.GetFileName(formFile.FileName));
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        return filePath;
    }

    private static string GetUploadFolderPath(string webrootPath, string folderName)
    {
        var folder = Path.Combine(webrootPath, "UploadedFiles", folderName);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        return folder;
    }
}
