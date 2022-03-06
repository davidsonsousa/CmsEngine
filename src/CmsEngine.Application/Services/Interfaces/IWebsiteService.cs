namespace CmsEngine.Application.Services.Interfaces;

public interface IWebsiteService : IDataTableService<Website>
{
    WebsiteEditModel SetupEditModel();
    Task<WebsiteEditModel> SetupEditModel(Guid id);
    Task<ReturnValue> Delete(Guid id);
    Task<ReturnValue> DeleteRange(Guid[] ids);
    Task<(IEnumerable<WebsiteTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
    Task<ReturnValue> Save(WebsiteEditModel categoryEditModel);
}
