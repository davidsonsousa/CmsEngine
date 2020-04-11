using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Data
{
    public class CmsEngineContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Email> Emails { get; set; }

        public CmsEngineContext(DbContextOptions<CmsEngineContext> options) : base(options)
        {
            //Database.SetInitializer(new CmsEngineInitializer());

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Model configuration
            builder.Entity<Website>(ModelConfiguration.ConfigureWebsite);
            builder.Entity<Page>(ModelConfiguration.ConfigurePage);
            builder.Entity<Post>(ModelConfiguration.ConfigurePost);
            builder.Entity<Tag>(ModelConfiguration.ConfigureTag);
            builder.Entity<Category>(ModelConfiguration.ConfigureCategory);
            builder.Entity<Email>(ModelConfiguration.ConfigureEmail);
            builder.Entity<PostCategory>(ModelConfiguration.ConfigurePostCategory);
            builder.Entity<PostTag>(ModelConfiguration.ConfigurePostTag);
            builder.Entity<PostApplicationUser>(ModelConfiguration.ConfigurePostApplicationUser);
            builder.Entity<PageApplicationUser>(ModelConfiguration.ConfigurePageApplicationUser);

            // TODO: Commenting seed out since it forces to remove all demo data
            //builder.Seed();

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var timeStamp = DateTime.Now;
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            string currentUsername = "";//HttpContext.Current?.User?.Identity?.Name;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateCreated").CurrentValue = timeStamp;
                    entry.Property("UserCreated").CurrentValue = currentUsername;
                }

                entry.Property("DateModified").CurrentValue = timeStamp;
                entry.Property("UserModified").CurrentValue = currentUsername;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
