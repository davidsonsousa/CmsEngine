using CmsEngine.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CmsEngine.Data
{
    public class CmsEngineContext : DbContext, IDbContext
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

        public CmsEngineContext(DbContextOptions<CmsEngineContext> options) : base(options)
        {
            //Database.SetInitializer(new CmsEngineInitializer());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Forces the database to use the type datetime2
            //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2").HasPrecision(0));

            // Model configuration
            modelBuilder.Entity<Website>(ModelConfiguration.ConfigureWebsite);
            modelBuilder.Entity<Page>(ModelConfiguration.ConfigurePage);
            modelBuilder.Entity<Post>(ModelConfiguration.ConfigurePost);
            modelBuilder.Entity<Tag>(ModelConfiguration.ConfigureTag);
            modelBuilder.Entity<Category>(ModelConfiguration.ConfigureCategory);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var saveTime = DateTime.Now;

            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentUsername = "";//HttpContext.Current?.User?.Identity?.Name;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateCreated").CurrentValue = saveTime;
                    entry.Property("UserCreated").CurrentValue = currentUsername;
                }

                entry.Property("DateModified").CurrentValue = saveTime;
                entry.Property("UserModified").CurrentValue = currentUsername;
            }

            return base.SaveChanges();
        }
    }
}
