namespace CmsEngine.Application.Extensions;

public static class EnumerableExtensions
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo?> NamePropertyCache = new();

    // Cache compiled getters per type to avoid PropertyInfo.GetValue allocations
    private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object?>> GetterCache = new();

    public static Expression<Func<T, bool>>? GetSearchExpression<T>(this IEnumerable<T> element, string searchValue, IEnumerable<PropertyInfo> properties)
    {
        var expressionFilter = properties.Select(property => new ExpressionFilter
        {
            PropertyName = property.Name,
            Operation = Operation.Contains,
            Value = searchValue
        }).ToList();
        return ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperator.Or);
    }

    public static IEnumerable<SelectListItem> PopulateSelectList<T>(this IEnumerable<T> items, IEnumerable<string>? selectedItems = null) where T : BaseViewModel
    {
        // Cache the Name property lookup per type to avoid repeated reflection
        var prop = NamePropertyCache.GetOrAdd(typeof(T), t => t.GetProperty("Name"));

        var selectedSet = selectedItems is null ? null : new HashSet<string>(selectedItems);

        // Materialize items to a list so we can pre-size the result and sort in-place
        var itemList = items is ICollection<T> coll ? coll : items.ToList();
        var result = new List<SelectListItem>(itemList.Count);

        Func<object, object?>? getter = null;
        if (prop != null)
        {
            // Try to get or create a compiled getter to avoid reflection costs
            getter = GetterCache.GetOrAdd(prop, p =>
            {
                var instanceParam = Expression.Parameter(typeof(object), "instance");
                var castInstance = Expression.Convert(instanceParam, p.DeclaringType!);
                var propertyAccess = Expression.Property(castInstance, p);
                var convertResult = Expression.Convert(propertyAccess, typeof(object));
                var lambda = Expression.Lambda<Func<object, object?>>(convertResult, instanceParam);
                return lambda.Compile();
            });
        }

        foreach (var x in itemList)
        {
            string text;
            if (prop is null)
            {
                // Fallback: try to use ToString()
                text = x?.ToString() ?? string.Empty;
            }
            else
            {
                var val = getter != null ? getter(x)! : prop.GetValue(x);
                text = val?.ToString() ?? string.Empty;
            }

            var vanity = x.VanityId.ToString();
            result.Add(new SelectListItem
            {
                Text = text,
                Value = vanity,
                Disabled = false,
                Selected = selectedSet?.Contains(vanity) ?? false
            });
        }

        // Sort in-place by Text to avoid LINQ allocations
        result.Sort((a, b) => string.Compare(a.Text, b.Text, StringComparison.Ordinal));
        return result;
    }

    public static IEnumerable<CheckboxEditModel> PopulateCheckboxList<T>(this IEnumerable<T> items, IEnumerable<string>? selectedItems = null) where T : BaseViewModel
    {
        var prop = NamePropertyCache.GetOrAdd(typeof(T), t => t.GetProperty("Name"));

        var selectedSet = selectedItems is null ? null : new HashSet<string>(selectedItems);

        var itemList = items is ICollection<T> coll ? coll : items.ToList();
        var result = new List<CheckboxEditModel>(itemList.Count);

        Func<object, object?>? getter = null;
        if (prop != null)
        {
            getter = GetterCache.GetOrAdd(prop, p =>
            {
                var instanceParam = Expression.Parameter(typeof(object), "instance");
                var castInstance = Expression.Convert(instanceParam, p.DeclaringType!);
                var propertyAccess = Expression.Property(castInstance, p);
                var convertResult = Expression.Convert(propertyAccess, typeof(object));
                var lambda = Expression.Lambda<Func<object, object?>>(convertResult, instanceParam);
                return lambda.Compile();
            });
        }

        foreach (var x in itemList)
        {
            string label;
            if (prop is null)
            {
                label = x?.ToString() ?? string.Empty;
            }
            else
            {
                var val = getter != null ? getter(x)! : prop.GetValue(x);
                label = val?.ToString() ?? string.Empty;
            }

            var vanity = x.VanityId.ToString();
            result.Add(new CheckboxEditModel
            {
                Label = label,
                Value = vanity,
                Enabled = true,
                Selected = selectedSet?.Contains(vanity) ?? false
            });
        }

        result.Sort((a, b) => string.Compare(a.Label, b.Label, StringComparison.Ordinal));
        return result;
    }
}
