using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.ViewModels.DataTableViewModels;

namespace CmsEngine.Domain.Services
{
    public interface ICategoryService : IDataTableService<Category>
    {
        CategoryEditModel SetupEditModel();
        Task<CategoryEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] id);
        Task<(IEnumerable<CategoryTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(CategoryEditModel categoryEditModel);
    }
}
