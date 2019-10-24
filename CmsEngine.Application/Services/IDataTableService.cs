using System.Collections.Generic;
using CmsEngine.Data.Entities;

namespace CmsEngine.Application.Services
{
    public interface IDataTableService<T> where T : BaseEntity
    {
        IEnumerable<T> FilterForDataTable(string searchValue, IEnumerable<T> items);
        IEnumerable<T> OrderForDataTable(int column, string direction, IEnumerable<T> items);
    }
}
