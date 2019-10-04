using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CmsEngine.Application.Attributes;
using CmsEngine.Application.ViewModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Core.Extensions;

namespace CmsEngine.Application.Helpers
{
    public static class DataTableHelper
    {
        public static DataTableViewModel BuildDataTable(IEnumerable<IViewModel> listItems, int recordsTotal, int recordsFiltered, int draw)
        {
            var listString = new List<List<string>>();

            foreach (var item in listItems)
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
            switch (property.PropertyType.Name)
            {
                case "DocumentStatus":
                    GeneralStatus generalStatus;
                    string documentStatus = item.GetType().GetProperty(property.Name).GetValue(item)?.ToString() ?? "";
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
                    var author = (UserViewModel)item.GetType().GetProperty(property.Name).GetValue(item);
                    return author?.FullName ?? ""; // TODO: Apply HTML encoding
                default:
                    return item.GetType().GetProperty(property.Name).GetValue(item)?.ToString() ?? "";  // TODO: Apply HTML encoding
            }
        }
    }
}
