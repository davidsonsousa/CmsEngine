namespace CmsEngine.Application.Helpers;

public static class DataTableHelper
{
    public static DataTableViewModel BuildDataTable(IEnumerable<IViewModel> listItems, int recordsTotal, int recordsFiltered, int draw, int start, int length)
    {
        var listString = new List<List<string>>();

        foreach (var item in listItems.Skip(start).Take(length))
        {
            // Get the properties which should appear in the DataTable
            var itemProperties = item.GetType()
                                     .GetProperties()
                                     .Where(p => Attribute.IsDefined(p, typeof(ShowOnDataTable)))
                                     .OrderBy(o => o.GetCustomAttributes(false).OfType<ShowOnDataTable>().First().Order);

            // An empty value must *always* be the first property because of the checkboxes
            var listPropertes = new List<string> { string.Empty };

            // Loop through and add the properties found
            foreach (var property in itemProperties)
            {
                listPropertes.Add(PrepareProperty(item, property));
            }

            // VanityId must *always* be the last property
            listPropertes.Add(item.VanityId.ToString());

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

    private static string PrepareProperty(IViewModel item, PropertyInfo property)
    {
        GeneralStatus generalStatus;
        var value = item.GetType().GetProperty(property.Name).GetValue(item);

        switch (property.PropertyType.Name)
        {
            case "DocumentStatus":
                var documentStatus = value?.ToString() ?? "";
                switch (documentStatus)
                {
                    case "Published":
                        generalStatus = GeneralStatus.Success;
                        break;
                    case "PendingApproval":
                        generalStatus = GeneralStatus.Warning;
                        break;
                    default:
                        generalStatus = GeneralStatus.Info;
                        break;
                }

                return $"<span class=\"badge badge-{generalStatus.ToString().ToLowerInvariant()}\">{documentStatus.ToEnum<DocumentStatus>().GetName()}</status-label>";
            case "UserViewModel":
                var author = (UserViewModel)value;
                return HtmlEncoder.Default.Encode(author?.FullName ?? "");
            case "Boolean":
                generalStatus = (bool)value ? GeneralStatus.Success : GeneralStatus.Danger;
                return $"<span class=\"badge badge-{generalStatus.ToString().ToLowerInvariant()}\">{((bool)value).ToYesNo().ToUpper()}</status-label>";
            default:
                return HtmlEncoder.Default.Encode(value?.ToString() ?? "");
        }
    }
}
