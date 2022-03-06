namespace CmsEngine.Data.Repositories.Interfaces;

public interface ICategoryRepository : IReadRepository<Category>, IDataModificationRepository<Category>, IDataModificationRangeRepository<Category>
{
    Task<Category> GetCategoryBySlug(string slug);
    Task<IEnumerable<Category>> GetCategoriesWithPostOrderedByName();
    Task<IEnumerable<Category>> GetCategoriesWithPostCountOrderedByName();
    Task<Category> GetCategoryBySlugWithPosts(string slug);
}
