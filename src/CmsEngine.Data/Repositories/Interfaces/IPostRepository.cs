namespace CmsEngine.Data.Repositories.Interfaces;

public interface IPostRepository : IReadRepository<Post>, IDataModificationRepository<Post>, IDataModificationRangeRepository<Post>
{
    Task<IEnumerable<Post>> GetPublishedPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy);
    Task<IEnumerable<Post>> GetByStatusOrderByDescending(DocumentStatus documentStatus);
    Task<(IEnumerable<Post> Items, int Count)> GetPublishedByCategoryForPagination(string categorySlug, int page, int articleLimit);
    Task<(IEnumerable<Post> Items, int Count)> GetPublishedByTagForPagination(string tagSlug, int page, int articleLimit);
    Task<(IEnumerable<Post> Items, int Count)> FindPublishedForPaginationOrderByDateDescending(int page, string searchTerm, int articleLimit);
    Task<(IEnumerable<Post> Items, int Count)> GetPublishedForPagination(int page, int articleLimit);
    Task<IEnumerable<Post>> GetPublishedLatestPosts(int count);
    Task<IEnumerable<Post>> GetForDataTable();
    Task<Post> GetForSavingById(Guid id);
    Task<Post> GetForEditingById(Guid id);
    Task<Post> GetBySlug(string slug);
    void RemoveRelatedItems(Post post);
}
