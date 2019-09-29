using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CmsEngineContext CmsEngineContext
        {
            get { return dbContext as CmsEngineContext; }
        }

        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategoriesById(Guid[] ids)
        {
            return await GetReadOnlyAsync(q => ids.Contains(q.VanityId));
        }

        public async Task<Category> GetCategoryBySlug(string slug)
        {
            return await Get(q => q.Slug == slug).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithPosts()
        {
            return await Get().Include(c => c.PostCategories).ToListAsync();
        }

        public async Task<Category> GetCategoryBySlugWithPosts(string slug)
        {
            return await Get(q => q.Slug == slug).Include(c => c.PostCategories).SingleOrDefaultAsync();
        }
    }
}
