namespace CmsEngine.Ui.Extensions;

public static class HtmlHelperExtensions
{
    public static string IsSelected(this IHtmlHelper htmlHelper, string? controllers = null, string? actions = null, string? slugs = null, string cssClass = "active")
    {
        var hasSlugs = !string.IsNullOrWhiteSpace(slugs) && CheckValues(htmlHelper, "slug", slugs);
        var hasActions = !string.IsNullOrWhiteSpace(actions) && CheckValues(htmlHelper, "action", actions);
        var hasController = !string.IsNullOrWhiteSpace(controllers) && CheckValues(htmlHelper, "controller", controllers);

        return hasSlugs || (hasActions && hasController)
               ? cssClass
               : string.Empty;
    }

    private static bool CheckValues(IHtmlHelper htmlHelper, string valueName, string? values)
    {
        var currentValue = htmlHelper.ViewContext.RouteData.Values[valueName] as string;

        if (string.IsNullOrWhiteSpace(currentValue))
        {
            return false;
        }

        var acceptedValues = (values ?? currentValue).Split(',').Select(x => x.Trim());

        return acceptedValues.Contains(currentValue, StringComparer.OrdinalIgnoreCase);
    }
}
