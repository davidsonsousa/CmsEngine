using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Website>> GetWebsites()
        {
            return await GetAll();
        }

        public async Task<Website> GetWebsiteById(Guid id)
        {
            return await GetById(id);
        }

        public async Task<Website> GetWebsiteByHost(string host)
        {
            return await Get(q => q.SiteUrl == host).SingleOrDefaultAsync();
        }

        public async Task<Website> GetFullWebsiteByHost(string host)
        {
            return await Get(q => q.SiteUrl == host)
                            .Include(i => i.Categories)
                            .Include(i => i.Pages)
                            .Include(i => i.Posts)
                            .Include(i => i.Tags)
                            .SingleOrDefaultAsync();
        }

        public async Task InsertWebsite(Website website)
        {
            await Insert(website);
        }

        public void UpdateWebsite(Website website)
        {
            Update(website);
        }

        public void DeleteWebsite(Website website)
        {
            Delete(website);
        }
    }
}
