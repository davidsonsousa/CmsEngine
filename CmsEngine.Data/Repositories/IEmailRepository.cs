using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IEmailRepository : IReadRepository<Email>, IDataModificationRepository<Email>
    {
        Task<IEnumerable<Email>> GetOrderedByDate();
    }
}
