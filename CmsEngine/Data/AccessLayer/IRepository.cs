using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CmsEngine.Data.AccessLayer
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets item based on condition and includes extra table
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="relatedTable"></param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// Gets items based on condition for read-only
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<T> GetReadOnly(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// Inserts a record
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);

        /// <summary>
        /// Inserts multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void InsertMany(IEnumerable<T> entities);

        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Updates multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void UpdateMany(IEnumerable<T> entities);

        /// <summary>
        /// Deletes record from database
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Deletes multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void DeleteMany(IEnumerable<T> entities);
    }
}
