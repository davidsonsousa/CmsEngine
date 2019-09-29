using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.Repositories
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        public CmsEngineContext CmsEngineContext
        {
            get { return dbContext as CmsEngineContext; }
        }

        public PageRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Page>> GetPagesOrderByDescending(Expression<Func<Page, DateTime>> orderBy)
        {
            return await Get().OrderByDescending(orderBy).ToListAsync();
        }
    }
}
