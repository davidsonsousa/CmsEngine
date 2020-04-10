using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(CmsEngineContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Email>> GetOrderedByDate()
        {
            return await Get().OrderByDescending(o => o.DateReceived)
                              .ToListAsync();
        }
    }
}
