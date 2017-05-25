using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CmsEngine.Data.AccessLayer
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;
        private readonly DbSet<T> _dbSet;
        private bool _disposed;

        public EfRepository()
        {

        }

        public EfRepository(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException("Repository - Context");
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IEnumerable<T> GetReadOnly(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.AsNoTracking().ToList();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> where, string include)
        {
            return _dbSet.Where(where).Include(include);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void InsertBulk(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void BulkUpdate(Expression<Func<T, bool>> where, Expression<Func<T, T>> update)
        {
            _dbSet.Where(where).Update(update);
        }

        public void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        private void Attach(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
