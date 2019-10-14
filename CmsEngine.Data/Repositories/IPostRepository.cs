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
        Task<IEnumerable<Post>> GetPublishedPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy);
        Task<IEnumerable<Post>> GetByStatusOrderByDescending(DocumentStatus documentStatus);
        Task<(IEnumerable<Post> Items, int Count)> GetPublishedByCategoryForPagination(string categorySlug, int page, int articleLimit);
        Task<(IEnumerable<Post> Items, int Count)> GetPublishedByTagForPagination(string tagSlug, int page, int articleLimit);
        Task<(IEnumerable<Post> Items, int Count)> FindPublishedForPaginationOrderByDateDescending(int page, string searchTerm, int articleLimit);
        Task<(IEnumerable<Post> Items, int Count)> GetPublishedForPagination(int page, int articleLimit);
        Task<IEnumerable<Post>> GetPublishedLatestPosts(int count);
        Task<IEnumerable<Post>> GetForDataTable();
        Task<Post> GetBySlug(string slug);
    }
}
