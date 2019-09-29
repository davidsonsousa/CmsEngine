using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public CmsEngineContext CmsEngineContext
        {
            get { return dbContext as CmsEngineContext; }
        }

        public TagRepository(DbContext context) : base(context)
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

        public async Task<IEnumerable<Tag>> GetTagsById(Guid[] ids)
        {
            return await GetReadOnlyAsync(q => ids.Contains(q.VanityId));
        }

        public async Task<IEnumerable<Tag>> GetTagsWithPosts()
        {
            return await Get().Include(t => t.PostTags).ToListAsync();
        }
    }
}
