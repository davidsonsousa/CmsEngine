using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Core;
using CmsEngine.Data.Entities;

namespace CmsEngine.Data.Repositories
{
    public interface IPageRepository : IReadRepository<Page>, IDataModificationRepository<Page>, IDataModificationRangeRepository<Page>
    {
        Task<IEnumerable<Page>> GetOrderByDescending(Expression<Func<Page, DateTime>> orderBy);
        Task<IEnumerable<Page>> GetByStatusOrderByDescending(DocumentStatus documentStatus);
        Task<IEnumerable<Page>> GetForDataTable();
        Task<Page> GetBySlug(string slug);
    }
}
