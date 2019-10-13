using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.ViewModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data.Entities;

namespace CmsEngine.Application.Services
{
    public interface ICategoryService : IDataTableService<Category>
    {
        CategoryEditModel SetupEditModel();
        Task<CategoryEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] ids);
        Task<(IEnumerable<CategoryTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(CategoryEditModel categoryEditModel);
        Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPostCount();
        Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPost();
    }
}
