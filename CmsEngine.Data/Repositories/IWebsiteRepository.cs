using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IWebsiteRepository : IReadRepository<Website>, IDataModificationRepository<Website>, IDataModificationRangeRepository<Website>
    {
        Task<IEnumerable<Website>> GetWebsitesById(Guid[] ids);
        Task<Website> GetWebsiteInstanceByHost(string host);
    }
}
