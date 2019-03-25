using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsEngine.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            string currentAction = (htmlHelper.ViewContext.RouteData.Values["action"] as string)?.ToLower();
            string currentController = (htmlHelper.ViewContext.RouteData.Values["controller"] as string)?.ToLower();

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',').Select(x => x.Trim().ToLower());
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',').Select(x => x.Trim().ToLower());

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }
    }
}
