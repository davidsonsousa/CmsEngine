using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data.AccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CmsEngineContext _ctx;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(CmsEngineContext context)
        {
            _ctx = context;

            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;

            // TODO: Make it flexible so it accepts other types of repository (example: ADO.NET, XML, JSON, etc.)
            var repository = new EfRepository<TEntity>(_ctx);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void Save()
        {
            try
            {
                _ctx.SaveChanges();
            }
            // TODO: Implement EF Core error handling
            //catch (DbEntityValidationException ex)
            //{
            //    // Retrieve the error messages as a list of strings.
            //    var errorMessages = ex.EntityValidationErrors
            //                            .SelectMany(x => x.ValidationErrors)
            //                            .Select(x => x.ErrorMessage);

            //    // Join the list to a single string.
            //    var fullErrorMessage = string.Join("; ", errorMessages);

            //    // Combine the original exception message with the new one.
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            //    // Throw a new DbEntityValidationException with the improved exception message.
            //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            //}
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.First().Reload();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
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
