using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public CmsEngineContext CmsEngineContext
        {
            get { return dbContext as CmsEngineContext; }
        }

        public PostRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Post>> GetPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy)
        {
            return await Get().OrderByDescending(orderBy).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByStatusOrderByDescending(DocumentStatus documentStatus)
        {
            return await Get(q => q.Status == documentStatus).OrderByDescending(o => o.PublishedOn).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsById(Guid[] ids)
        {
            return await GetReadOnlyAsync(q => ids.Contains(q.VanityId));
        }
    }
}
