using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CmsEngine.Data.AccessLayer
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// Gets item based on condition and includes extra table
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, int count = 0);

        /// <summary>
        /// Gets items based on condition for read-only
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetReadOnly(Expression<Func<T, bool>> filter = null);

        Task<T> GetById(int id);

        Task<T> GetById(Guid id);

        Task<int> Count(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// Inserts a record
        /// </summary>
        /// <param name="entity"></param>
        Task Insert(T entity);

        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Deletes record from database
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
    }
}
