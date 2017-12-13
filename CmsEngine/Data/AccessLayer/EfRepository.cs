using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CmsEngine.Data.AccessLayer
{
    internal class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly CmsEngineContext _context;
        private readonly DbSet<T> _dbSet;
        private bool _disposed;

        public EfRepository()
        {

        }

        public EfRepository(CmsEngineContext context)
        {
            _context = context ?? throw new ArgumentNullException("Repository - Context");
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, string relatedTable = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(relatedTable))
            {
                query = query.Include(relatedTable);
            }

            return query.AsNoTracking();
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

        public void BulkUpdate(Expression<Func<T, bool>> where, Action<T> update)
        {
            // TODO: There must be a better way to do this
            var items = _dbSet.Where(where).ToList();
            items.ForEach(update);
        }

        public void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        private void Attach(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
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
