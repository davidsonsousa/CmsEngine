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

        public Task<IEnumerable<Category>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategoriesWithPosts()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryBySlug(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryBySlugWithPosts(string slug)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetCategoriesById(Guid[] ids)
        {
            return await GetReadOnly(q => ids.Contains(q.VanityId));
        }
    }
}
