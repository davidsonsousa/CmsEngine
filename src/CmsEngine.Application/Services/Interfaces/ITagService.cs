namespace CmsEngine.Application.Services.Interfaces;

public interface ITagService : IDataTableService<Tag>, IDisposable
{
    TagEditModel SetupEditModel();
    Task<TagEditModel> SetupEditModel(Guid id);
    Task<ReturnValue> Delete(Guid id);
    Task<ReturnValue> DeleteRange(Guid[] ids);
    Task<(IEnumerable<TagTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
    Task<ReturnValue> Save(TagEditModel tagEditModel);
    Task<IEnumerable<TagViewModel>> GetAllTags();
}
