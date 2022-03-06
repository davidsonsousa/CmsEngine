namespace CmsEngine.Application.Extensions;

public static class EnumerableExtensions
{
    public static Expression<Func<T, bool>> GetSearchExpression<T>(this IEnumerable<T> element, string searchValue, IEnumerable<PropertyInfo> properties)
    {
        var expressionFilter = new List<ExpressionFilter>();

        foreach (var property in properties)
        {
            expressionFilter.Add(new ExpressionFilter
            {
                PropertyName = property.Name,
                Operation = Operation.Contains,
                Value = searchValue
            });
        }

        return ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperator.Or);
    }

    public static IEnumerable<SelectListItem> PopulateSelectList<T>(this IEnumerable<T> items, IEnumerable<string> selectedItems = null) where T : BaseViewModel
    {
        return items.Select(x => new SelectListItem
        {
            Text = x.GetType().GetProperty("Name").GetValue(x).ToString(),
            Value = x.VanityId.ToString(),
            Disabled = false,
            Selected = selectedItems?.Contains(x.VanityId.ToString()) ?? false
        }).OrderBy(o => o.Text);
    }

    public static IEnumerable<CheckboxEditModel> PopulateCheckboxList<T>(this IEnumerable<T> items, IEnumerable<string> selectedItems = null) where T : BaseViewModel
    {
        return items.Select(x => new CheckboxEditModel
        {
            Label = x.GetType().GetProperty("Name").GetValue(x).ToString(),
            Value = x.VanityId.ToString(),
            Enabled = true,
            Selected = selectedItems?.Contains(x.VanityId.ToString()) ?? false
        }).OrderBy(o => o.Label);
    }
}
