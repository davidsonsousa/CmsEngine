using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy);
    }
}
