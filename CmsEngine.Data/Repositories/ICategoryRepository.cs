using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface ICategoryRepository : IDataModificationRepository<Category>, IDataModificationRangeRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetCategoriesById(Guid[] ids);
        Task<Category> GetCategoryById(Guid id);
        Task<Category> GetCategoryBySlug(string slug);
        Task<IEnumerable<Category>> GetCategoriesWithPosts();
        Task<Category> GetCategoryBySlugWithPosts(string slug);
    }
}
