using Microsoft.AspNetCore.Mvc;

namespace CmsEngine.Ui.Areas.Cms.Controllers
{
    public class BaseController : Controller
    {
        protected void SetupMessages(string pageTitle, string description = "", string panelTitle = "")
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = description;
            ViewBag.PanelTitle = panelTitle;
        }

        protected void SetupMessages(string pageTitle, string modelError, string generalError, string description = "", string panelTitle = "")
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = description;
            ViewBag.PanelTitle = panelTitle;

            if (!string.IsNullOrWhiteSpace(modelError))
            {
                ModelState.AddModelError("", modelError);
            }

            TempData["DangerMessage"] = generalError;
        }
    }
}
