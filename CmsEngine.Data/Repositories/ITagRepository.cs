using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface ITagRepository : IReadRepository<Tag>, IDataModificationRepository<Tag>, IDataModificationRangeRepository<Tag>
    {
        Task<Tag> GetTagBySlug(string slug);
        Task<IEnumerable<Tag>> GetTagsWithPosts();
        Task<Tag> GetTagBySlugWithPosts(string slug);
    }
}
