using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.ViewModels.DataTableViewModels;

namespace CmsEngine.Domain.Services
{
    public interface IPageService : IDataTableService<Page>
    {
        PageEditModel SetupEditModel();
        Task<PageEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] ids);
        Task<(IEnumerable<PageTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(PageEditModel pageEditModel);
        Task<IEnumerable<PageEditModel>> GetByStatus(DocumentStatus documentStatus, int count = 0);
    }
}
