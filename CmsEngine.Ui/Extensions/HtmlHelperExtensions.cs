using System.Linq;
using CmsEngine.Core.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsEngine.Ui.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            Guard.ThrownExceptionIfNull(htmlHelper, nameof(htmlHelper));

            string currentAction = (htmlHelper.ViewContext.RouteData.Values["action"] as string)?.ToLower();
            string currentController = (htmlHelper.ViewContext.RouteData.Values["controller"] as string)?.ToLower();
            var acceptedActions = (actions ?? currentAction).Split(',').Select(x => x.Trim().ToLower());
            var acceptedControllers = (controllers ?? currentController).Split(',').Select(x => x.Trim().ToLower());
            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }
    }
}
