using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class WebsiteRepository : Repository<Website>, IWebsiteRepository
    {
        public CmsEngineContext CmsEngineContext
        {
            get { return dbContext as CmsEngineContext; }
        }

        public WebsiteRepository(DbContext context) : base(context)
        {

        }

        public async Task<Website> GetWebsiteInstanceByHost(string host)
        {
            return await Get(q => q.SiteUrl == host)
                            .Include(i => i.Categories)
                            .Include(i => i.Pages)
                            .Include(i => i.Posts)
                            .Include(i => i.Tags)
                            .SingleOrDefaultAsync();
        }
    }
}
