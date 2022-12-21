namespace CmsEngine.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CmsEngineContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryBySlug(string slug)
    {
        return await Get(q => q.Slug == slug).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithPostCountOrderedByName()
    {
        return await Get().Include(c => c.PostCategories)
                          .Where(q => q.PostCategories.Any(pc => pc.Post.Status == DocumentStatus.Published && pc.Post.IsDeleted == false))
                          .Select(c => new Category
                          {
                              VanityId = c.VanityId,
                              Name = c.Name,
                              Slug = c.Slug,
                              PostCount = c.PostCategories.Count()
                          })
                          .OrderBy(o => o.Name).ToListAsync();
    }

    public async Task<Category?> GetCategoryBySlugWithPosts(string slug)
    {
        return await Get(q => q.Slug == slug).Include(c => c.PostCategories).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithPostOrderedByName()
    {
        return await Get().Include(c => c.PostCategories)
                            .ThenInclude(pc => pc.Post)
                          .Where(q => q.PostCategories.Any(pc => pc.Post.Status == DocumentStatus.Published && pc.Post.IsDeleted == false))
                          .Select(c => new Category
                          {
                              VanityId = c.VanityId,
                              Name = c.Name,
                              Slug = c.Slug,
                              Posts = c.PostCategories.Select(pc => pc.Post).Where(q => q.IsDeleted == false).Select(p => new Post
                              {
                                  VanityId = p.VanityId,
                                  Title = p.Title,
                                  Description = p.Description,
                                  Slug = p.Slug,
                                  PublishedOn = p.PublishedOn
                              })
                          })
                          .OrderBy(o => o.Name).ToListAsync();
    }
}
