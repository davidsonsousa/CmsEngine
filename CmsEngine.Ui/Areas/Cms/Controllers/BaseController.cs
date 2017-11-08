using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    public class BaseController : Controller
    {
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
    }
}
