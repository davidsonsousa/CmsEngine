using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    }
}
