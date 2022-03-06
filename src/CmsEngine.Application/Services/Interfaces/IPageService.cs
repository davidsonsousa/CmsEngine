namespace CmsEngine.Application.Services.Interfaces;

public interface IPageService : IDataTableService<Page>
{
    PageEditModel SetupEditModel();
    Task<PageEditModel> SetupEditModel(Guid id);
    Task<ReturnValue> Delete(Guid id);
    Task<ReturnValue> DeleteRange(Guid[] ids);
    Task<(IEnumerable<PageTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
    Task<ReturnValue> Save(PageEditModel pageEditModel);
    Task<PageViewModel> GetBySlug(string slug);
    Task<IEnumerable<PageViewModel>> GetAllPublished();
}
