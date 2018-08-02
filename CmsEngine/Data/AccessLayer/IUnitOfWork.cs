using System;
using CmsEngine.Data.Models;

namespace CmsEngine.Data.AccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Page> Pages { get; }
        IRepository<Post> Posts { get; }
        IRepository<Tag> Tags { get; }
        IRepository<Website> Websites { get; }

        /// <summary>
        /// Get repository according to the type
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel;

        /// <summary>
        /// Saves all pending changes into the database
        /// </summary>
        void Save();
    }
}
