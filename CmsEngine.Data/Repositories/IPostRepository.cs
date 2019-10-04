using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IPostRepository : IReadRepository<Post>, IDataModificationRepository<Post>, IDataModificationRangeRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy);
        Task<IEnumerable<Post>> GetByStatusOrderByDescending(DocumentStatus documentStatus);
        Task<IEnumerable<Post>> GetPostsById(Guid[] ids);
    }
}
