using System;

namespace CmsEngine.Data.AccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository according to the type
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all pending changes into the database
        /// </summary>
        void Save();
    }
}
