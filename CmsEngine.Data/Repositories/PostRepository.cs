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
            var posts = Get(q => q.Status == DocumentStatus.Published).Include(p => p.PostCategories)
                                                                      .ThenInclude(pc => pc.Category)
                                                                      .OrderByDescending(o => o.PublishedOn)
                                                                      .Where(q => q.PostCategories.Any(pc => pc.Category.Slug == categorySlug));

            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> GetPublishedByTagForPagination(string tagSlug, int page, int articleLimit)
        {
            var posts = Get(q => q.Status == DocumentStatus.Published).Include(p => p.PostTags)
                                                                      .ThenInclude(pt => pt.Tag)
                                                                      .OrderByDescending(o => o.PublishedOn)
                                                                      .Where(q => q.PostTags.Any(pc => pc.Tag.Slug == tagSlug));

            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> FindPublishedForPaginationOrderByDateDescending(int page, string searchTerm, int articleLimit)
        {
            var posts = string.IsNullOrWhiteSpace(searchTerm)
                        ? Get(q => q.Status == DocumentStatus.Published)
                            .Include(p => p.PostApplicationUsers)
                                .ThenInclude(pau => pau.ApplicationUser)
                        : Get(q => (q.Title.Contains(searchTerm) || q.DocumentContent.Contains(searchTerm)) && q.Status == DocumentStatus.Published)
                            .Include(p => p.PostApplicationUsers)
                                .ThenInclude(pau => pau.ApplicationUser);

            int count = await posts.CountAsync();
            var items = await posts.Select(p => new Post
            {
                VanityId = p.VanityId,
                Title = p.Title,
                Slug = p.Slug,
                Description = p.Description,
                HeaderImage = p.HeaderImage,
                PublishedOn = p.PublishedOn,
                Categories = p.PostCategories.Select(pc => pc.Category).Select(c => new Category
                {
                    VanityId = c.VanityId,
                    Name = c.Name,
                    Slug = c.Slug
                }),
                ApplicationUsers = p.PostApplicationUsers.Select(pau => pau.ApplicationUser).Select(au => new ApplicationUser
                {
                    Id = au.Id,
                    Name = au.Name,
                    Surname = au.Surname,
                    Email = au.Email
                })
            }).OrderByDescending(o => o.PublishedOn).Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<(IEnumerable<Post> Items, int Count)> GetPublishedForPagination(int page, int articleLimit)
        {
            var posts = Get(q => q.Status == DocumentStatus.Published).OrderByDescending(o => o.PublishedOn)
                            .Include(p => p.PostApplicationUsers)
                            .ThenInclude(pau => pau.ApplicationUser);
            int count = posts.Count();
            var items = await posts.Skip((page - 1) * articleLimit).Take(articleLimit).ToListAsync();

            return (items, count);
        }

        public async Task<IEnumerable<Post>> GetPublishedLatestPosts(int count)
        {
            return await Get(q => q.Status == DocumentStatus.Published)
                            .Include(p => p.PostCategories)
                                .ThenInclude(pc => pc.Category)
                            .Include(p => p.PostApplicationUsers)
                                .ThenInclude(pau => pau.ApplicationUser)
                            .Select(p => new Post
                            {
                                VanityId = p.VanityId,
                                Title = p.Title,
                                Slug = p.Slug,
                                Description = p.Description,
                                HeaderImage = p.HeaderImage,
                                PublishedOn = p.PublishedOn,
                                Categories = p.PostCategories.Select(pc => pc.Category).Select(c => new Category
                                {
                                    VanityId = c.VanityId,
                                    Name = c.Name,
                                    Slug = c.Slug
                                }),
                                ApplicationUsers = p.PostApplicationUsers.Select(pau => pau.ApplicationUser).Select(au => new ApplicationUser
                                {
                                    Id = au.Id,
                                    Name = au.Name,
                                    Surname = au.Surname,
                                    Email = au.Email
                                })
                            })
                            .OrderByDescending(o => o.PublishedOn).Take(count).ToListAsync();
        }
    }
}
