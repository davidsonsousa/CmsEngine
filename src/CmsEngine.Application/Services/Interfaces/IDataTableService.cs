namespace CmsEngine.Application.Services.Interfaces;

public interface IDataTableService<T> where T : BaseEntity
{
    IQueryable<T> FilterForDataTable(string searchValue, IQueryable<T> items);
    IQueryable<T> OrderForDataTable(int column, string direction, IQueryable<T> items);
}
