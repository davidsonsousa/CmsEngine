using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMSEngine.Data.AccessLayer
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> GetReadOnly(Expression<Func<T, bool>> filter = null);
        void Insert(T entity);
        void InsertBulk(IEnumerable<T> entities);
        void Update(T entity);
        void BulkUpdate(Expression<Func<T, bool>> where, Expression<Func<T, T>> update);
        void Delete(T entity);
    }
}
