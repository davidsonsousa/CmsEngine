using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CmsEngine.Data.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all records which were not marked as deleted (IsDeleted == false)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

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
        Task<IEnumerable<TEntity>> GetReadOnlyAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets record by vanity id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(Guid id);
    }
}
