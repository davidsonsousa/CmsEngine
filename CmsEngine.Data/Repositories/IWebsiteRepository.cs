using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IWebsiteRepository : IReadRepository<Website>, IDataModificationRepository<Website>, IDataModificationRangeRepository<Website>
    {
        Task<Website> GetWebsiteInstanceByHost(string host);
        Task<IEnumerable<Website>> GetForDataTable();
    }
}
