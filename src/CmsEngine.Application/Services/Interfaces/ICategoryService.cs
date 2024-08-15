namespace CmsEngine.Application.Services.Interfaces;

public interface ICategoryService : IDataTableService<Category>, IDisposable
{
    CategoryEditModel SetupEditModel();
    Task<CategoryEditModel> SetupEditModel(Guid id);
    Task<ReturnValue> Delete(Guid id);
    Task<ReturnValue> DeleteRange(Guid[] ids);
    Task<(IEnumerable<CategoryTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
    Task<ReturnValue> Save(CategoryEditModel categoryEditModel);
    Task<int> GetCategoryCount();
    Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPostCount();
    Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPost();
}
