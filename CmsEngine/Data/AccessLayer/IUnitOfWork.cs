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
        /// Saves all pending changes into the database
        /// </summary>
        void Save();
    }
}
