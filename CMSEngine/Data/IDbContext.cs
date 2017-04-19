using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CmsEngine.Data
{
    public interface IDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
