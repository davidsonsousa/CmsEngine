/// Copied from here: https://stackoverflow.com/questions/52220090/what-is-the-asp-net-core-equivalent-for-html-isselected

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsEngine.Ui.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string controllers = null, string actions = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controllers))
            {
                controllers = currentController;
            }

            if (string.IsNullOrEmpty(actions))
            {
                actions = currentAction;
            }

            return controllers == currentController && actions == currentAction ? cssClass : string.Empty;
        }
    }
}
