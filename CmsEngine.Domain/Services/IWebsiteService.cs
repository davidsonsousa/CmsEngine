using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.ViewModels.DataTableViewModels;

namespace CmsEngine.Domain.Services
{
    public interface IWebsiteService : IDataTableService<Website>
    {
        WebsiteEditModel SetupEditModel();
        Task<WebsiteEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] ids);
        Task<(IEnumerable<WebsiteTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(WebsiteEditModel categoryEditModel);
    }
}
