namespace CmsEngine.Data.Repositories.Interfaces;

public interface ITagRepository : IReadRepository<Tag>, IDataModificationRepository<Tag>, IDataModificationRangeRepository<Tag>, IDisposable
{
    Task<Tag?> GetTagBySlug(string slug);

    Task<IEnumerable<Tag>> GetTagsWithPosts();

    Task<Tag?> GetTagBySlugWithPosts(string slug);
}
