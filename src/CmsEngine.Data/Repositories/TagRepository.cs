namespace CmsEngine.Data.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(CmsEngineContext context) : base(context)
    {

    }

    public async Task<Tag> GetTagBySlug(string slug)
    {
        return await Get(q => q.Slug == slug).SingleOrDefaultAsync();
    }

    public async Task<Tag> GetTagBySlugWithPosts(string slug)
    {
        return await Get(q => q.Slug == slug).Include(t => t.PostTags).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsWithPosts()
    {
        return await Get().Include(t => t.PostTags).ToListAsync();
    }
}
