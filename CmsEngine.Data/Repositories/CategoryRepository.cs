using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CmsEngineContext context) : base(context)
        {
        }

        public async Task<Category> GetCategoryBySlug(string slug)
        {
            return await Get(q => q.Slug == slug).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithPostsOrderedByName()
        {
            return await Get().Include(c => c.PostCategories).ThenInclude(pc => pc.Post).OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<Category> GetCategoryBySlugWithPosts(string slug)
        {
            return await Get(q => q.Slug == slug).Include(c => c.PostCategories).SingleOrDefaultAsync();
        }
    }
}
