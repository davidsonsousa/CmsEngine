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
        public PostRepository(CmsEngineContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Post>> GetPublishedPostsOrderByDescending(Expression<Func<Post, DateTime>> orderBy)
        {
            return await Get(q => q.Status == DocumentStatus.Published).OrderByDescending(orderBy).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByStatusOrderByDescending(DocumentStatus documentStatus)
        {
            return await Get(q => q.Status == documentStatus).OrderByDescending(o => o.PublishedOn).ToListAsync();
        }

        public async Task<Post> GetBySlug(string slug)
        {
            return await Get(q => q.Slug == slug).SingleOrDefaultAsync();
        }

        public async Task<(IEnumerable<Post> Items, int Count)> GetPublishedByCategoryForPagination(string categorySlug, int page, int articleLimit)
        {
            var posts = Get(q => q.Status == DocumentStatus.Published).OrderByDescending(o => o.PublishedOn)
                                                                      .Where(q => q.PostCategories.Any(pc => pc.Category.Slug == categorySlug));

            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> GetPublishedByTagForPagination(string tagSlug, int page, int articleLimit)
        {
            var posts = Get(q => q.Status == DocumentStatus.Published).OrderByDescending(o => o.PublishedOn)
                                                                      .Where(q => q.PostTags.Any(pc => pc.Tag.Slug == tagSlug));
            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> FindPublishedForPaginationOrderByDateDescending(int page, string searchTerm, int articleLimit)
        {
            var posts = string.IsNullOrWhiteSpace(searchTerm)
                        ? Get(q => q.Status == DocumentStatus.Published)
                        : Get(q => (q.Title.Contains(searchTerm) || q.DocumentContent.Contains(searchTerm)) && q.Status == DocumentStatus.Published);

            int count = posts.Count();
            var items = await posts.OrderBy(o => o.PublishedOn).Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> GetPublishedForPagination(int page, int articleLimit)
        {
            var posts = Get(q => q.Status == DocumentStatus.Published).OrderByDescending(o => o.PublishedOn);
            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }
    }
}
