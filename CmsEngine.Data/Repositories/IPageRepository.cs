using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetPagesOrderByDescending(Expression<Func<Page, DateTime>> orderBy);
    }
}
