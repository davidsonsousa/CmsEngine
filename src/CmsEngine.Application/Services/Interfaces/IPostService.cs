namespace CmsEngine.Application.Services.Interfaces;

public interface IPostService : IDataTableService<Post>, IDisposable
{
    Task<PostEditModel> SetupEditModel();
    Task<PostEditModel> SetupEditModel(Guid id);
    Task<ReturnValue> Delete(Guid id);
    Task<ReturnValue> DeleteRange(Guid[] ids);
    Task<(IEnumerable<PostTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters);
    Task<ReturnValue> Save(PostEditModel postEditModel);
    Task<IEnumerable<PostEditModel>> GetPublishedOrderedByDate(int count = 0);
    Task<PostViewModel> GetBySlug(string slug);
    Task<int> GetPostCount();
    Task<PaginatedList<PostViewModel>> GetPublishedByCategoryForPagination(string categorySlug, int page = 1);
    Task<PaginatedList<PostViewModel>> GetPublishedByTagForPagination(string tagSlug, int page = 1);
    Task<PaginatedList<PostViewModel>> GetPublishedForPagination(int page = 1);
    Task<IEnumerable<PostViewModel>> GetPublishedLatestPosts(int count);
    Task<PaginatedList<PostViewModel>> FindPublishedForPaginationOrderByDateDescending(string searchTerm = "", int page = 1);
}
