using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IWebsiteRepository
    {
        Task<IEnumerable<Website>> GetWebsites();
        Task<Website> GetWebsiteById(Guid id);
        Task<Website> GetWebsiteByHost(string host);
        Task<Website> GetFullWebsiteByHost(string host);
        Task InsertWebsite(Website website);
        void UpdateWebsite(Website website);
        void DeleteWebsite(Website website);
    }
}
