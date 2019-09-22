using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CmsEngine.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CmsEngine.Data.AccessLayer
{
    internal class EfRepository<T> : IRepository<T> where T : BaseModel
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

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, int count = 0)
        {
            var query = GetAll(q => q.IsDeleted == false);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetReadOnly(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Get(q => q.Id == id).SingleOrDefaultAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await Get(q => q.VanityId == id).SingleOrDefaultAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                return await query.CountAsync(filter);
            }

            return await query.CountAsync();
        }

        public async Task Insert(T entity)
        {
            if (entity != null)
            {
                await _dbSet.AddAsync(entity);
            }
        }

        public async Task InsertMany(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                await _dbSet.AddRangeAsync(entities);
            }
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void UpdateMany(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                _context.UpdateRange(entities);
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public void DeleteMany(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                _context.RemoveRange(entities);
            }
        }

        private void Attach(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
        }

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
    }
}
