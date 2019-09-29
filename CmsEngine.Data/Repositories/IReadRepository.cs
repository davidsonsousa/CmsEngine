using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CmsEngine.Data.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// Gets item based on condition and includes extra table
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, int count = 0);

        /// <summary>
        /// Gets items based on condition for read-only
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetReadOnly(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> GetById(int id);

        Task<TEntity> GetById(Guid id);
    }
}
