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
    public interface ITagService : IDataTableService<Tag>
    {
        TagEditModel SetupEditModel();
        Task<TagEditModel> SetupEditModel(Guid id);
        Task<ReturnValue> Delete(Guid id);
        Task<ReturnValue> DeleteRange(Guid[] ids);
        Task<(IEnumerable<TagTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
        Task<ReturnValue> Save(TagEditModel tagEditModel);
        Task<IEnumerable<TagViewModel>> GetAllTags();
    }
}
