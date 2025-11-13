namespace CmsEngine.Application.Helpers;

public static class DataTableHelper
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    // Cache compiled getters per property to avoid repeated PropertyInfo.GetValue allocations
    private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object?>> GetterCache = new();

    // Cache for DocumentStatus display names (GetName()) to avoid reflection per item
    private static readonly ConcurrentDictionary<DocumentStatus, string> DocumentStatusNameCache = new();

    [ThreadStatic]
    private static StringBuilder? _sb;

    public static DataTableViewModel BuildDataTable(IEnumerable<IViewModel> listItems, int recordsTotal, int recordsFiltered, int draw, int start, int length)
    {
        var listString = new List<List<string>>();

        foreach (var item in listItems.Skip(start).Take(length))
        {
            var itemType = item.GetType();

            // Get cached, ordered properties for the type
            var itemProperties = PropertyCache.GetOrAdd(itemType, t =>
                t.GetProperties()
                 .Where(p => Attribute.IsDefined(p, typeof(ShowOnDataTable)))
                 .OrderBy(p => ((ShowOnDataTable?)p.GetCustomAttributes(false).OfType<ShowOnDataTable>().FirstOrDefault())?.Order ?? 0)
                 .ToArray());

            // Pre-size the list: checkbox + properties + vanity id
            var listPropertes = new List<string>(itemProperties.Length + 2) { string.Empty };

            // Loop through and add the properties found
            foreach (var property in itemProperties)
            {
                // Use compiled getter to avoid reflection overhead
                var getter = GetterCache.GetOrAdd(property, p => CreateGetter(p));
                var value = getter(item);
                listPropertes.Add(PrepareProperty(value, property));
            }

            // VanityId must *always* be the last property
            if (item is CmsEngine.Application.Models.ViewModels.BaseViewModel vm)
            {
                listPropertes.Add(vm.VanityIdString);
            }
            else
            {
                listPropertes.Add(item.VanityId.ToString());
            }

            listString.Add(listPropertes);
        }

        return new DataTableViewModel
        {
            Data = listString,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Draw = draw
        };
    }

    private static Func<object, object?> CreateGetter(PropertyInfo p)
    {
        var instanceParam = Expression.Parameter(typeof(object), "instance");
        var castInstance = Expression.Convert(instanceParam, p.DeclaringType!);
        var propertyAccess = Expression.Property(castInstance, p);
        var convertResult = Expression.Convert(propertyAccess, typeof(object));
        var lambda = Expression.Lambda<Func<object, object?>>(convertResult, instanceParam);
        return lambda.Compile();
    }

    private static string PrepareProperty(object? value, PropertyInfo property)
    {
        if (value == null)
        {
            return string.Empty;
        }

        // Ensure thread-local StringBuilder exists
        _sb ??= new StringBuilder(256);
        _sb.Clear();

        // Use the actual property type where possible to avoid string-based type checks
        var propType = property.PropertyType;

        if (propType == typeof(DocumentStatus) || value is DocumentStatus)
        {
            DocumentStatus documentStatus = value is DocumentStatus ds ? ds : value.ToString().ToEnum<DocumentStatus>();

            GeneralStatus generalStatus;
            switch (documentStatus)
            {
                case DocumentStatus.Published:
                    generalStatus = GeneralStatus.Success;
                    break;
                case DocumentStatus.PendingApproval:
                    generalStatus = GeneralStatus.Warning;
                    break;
                default:
                    generalStatus = GeneralStatus.Info;
                    break;
            }

            // Get cached display name for the enum
            var displayName = DocumentStatusNameCache.GetOrAdd(documentStatus, ds => ds.GetName());

            // Build the span using StringBuilder to reduce intermediate allocations
            _sb.Append("<span class=\"badge text-bg-");
            _sb.Append(generalStatus.ToString().ToLowerInvariant());
            _sb.Append("\">");
            _sb.Append(displayName);
            _sb.Append("</status-label>");

            return _sb.ToString();
        }

        if (propType.Name == "UserViewModel" || value is UserViewModel)
        {
            var author = value as UserViewModel;
            return HtmlEncoder.Default.Encode(author?.FullName ?? string.Empty);
        }

        if (propType == typeof(bool) || value is bool)
        {
            var b = value is bool vb && vb;
            var generalStatus = b ? GeneralStatus.Success : GeneralStatus.Danger;

            _sb.Append("<span class=\"badge text-bg-");
            _sb.Append(generalStatus.ToString().ToLowerInvariant());
            _sb.Append("\">");
            _sb.Append((b).ToYesNo().ToUpper());
            _sb.Append("</status-label>");

            return _sb.ToString();
        }

        // Default: HTML-encode the string representation
        return HtmlEncoder.Default.Encode(value?.ToString() ?? string.Empty);
    }
}
