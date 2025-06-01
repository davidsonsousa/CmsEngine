namespace CmsEngine.Data;

public class CmsEngineContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public DbSet<Website> Websites => Set<Website>();

    public DbSet<Page> Pages => Set<Page>();

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Email> Emails => Set<Email>();

    public CmsEngineContext(DbContextOptions<CmsEngineContext> options, IHttpContextAccessor hca) : base(options)
    {
        //Database.SetInitializer(new CmsEngineInitializer());

        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        httpContextAccessor = hca;
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

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        // TODO: Make it testable
        var currentUsername = httpContextAccessor.HttpContext?.User.Identity?.Name ?? "anonymous";

        var timeStamp = DateTime.UtcNow;
        var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(Auditing.DateCreated).CurrentValue = timeStamp;
                entry.Property(Auditing.UserCreated).CurrentValue = currentUsername;
            }

            entry.Property(Auditing.DateModified).CurrentValue = timeStamp;
            entry.Property(Auditing.UserModified).CurrentValue = currentUsername;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
