using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data.Entities;

namespace CmsEngine.Application.Services
{
    public interface IPostService : IDataTableService<Post>
    {
        PostEditModel SetupEditModel();
        Task<PostEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] ids);
        Task<(IEnumerable<PostTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(PostEditModel postEditModel);
        Task<IEnumerable<PostEditModel>> GetByStatus(DocumentStatus documentStatus, int count = 0);
    }
}
