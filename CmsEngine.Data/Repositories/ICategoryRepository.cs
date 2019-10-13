using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface ICategoryRepository : IReadRepository<Category>, IDataModificationRepository<Category>, IDataModificationRangeRepository<Category>
    {
        Task<Category> GetCategoryBySlug(string slug);
        Task<IEnumerable<Category>> GetCategoriesWithPostCountOrderedByName();
        Task<Category> GetCategoryBySlugWithPosts(string slug);
    }
}
