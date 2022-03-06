namespace CmsEngine.Data.Entities;

public static class ModelConfiguration
{
    public static void ConfigureWebsite(EntityTypeBuilder<Website> b)
    {
        // Fields
        b.HasKey(model => model.Id);

        b.Property(model => model.Name)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(model => model.Tagline)
            .HasMaxLength(200);

        b.Property(model => model.Culture)
            .HasMaxLength(5)
            .IsRequired();

        b.Property(model => model.UrlFormat)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(model => model.DateFormat)
            .HasMaxLength(10)
            .IsRequired();

        b.Property(model => model.SiteUrl)
            .HasMaxLength(250)
            .IsRequired();

        b.Property(model => model.ArticleLimit)
            .IsRequired();

        b.Property(model => model.Address)
            .HasMaxLength(250);

        b.Property(model => model.Phone)
            .HasMaxLength(20);

        b.Property(model => model.Email)
            .HasMaxLength(250);

        b.Property(model => model.Facebook)
            .HasMaxLength(20);

        b.Property(model => model.Twitter)
            .HasMaxLength(20);

        b.Property(model => model.Instagram)
            .HasMaxLength(20);

        b.Property(model => model.LinkedIn)
            .HasMaxLength(20);

        b.Property(model => model.FacebookAppId)
            .HasMaxLength(30);

        b.Property(model => model.FacebookApiVersion)
            .HasMaxLength(10);

        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);

        // Relationships
        b.HasMany(model => model.Posts);
        b.HasMany(model => model.Pages);
        b.HasMany(model => model.Tags);
        b.HasMany(model => model.Categories);
    }

    public static void ConfigurePage(EntityTypeBuilder<Page> b)
    {
        // Fields
        b.HasKey(model => model.Id);

        b.Property(model => model.Title)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(model => model.Slug)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(model => model.Description)
            .HasMaxLength(150)
            .IsRequired();

        //b.Property(model => model.DocumentContent)
        //    .IsRequired();

        b.Property(model => model.PublishedOn)
            .IsRequired();

        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);

        // Relationships
        b.HasOne(model => model.Website)
            .WithMany(model => model.Pages);
    }

    public static void ConfigurePost(EntityTypeBuilder<Post> b)
    {
        // Fields
        b.HasKey(model => model.Id);

        b.Property(model => model.Title)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(model => model.Slug)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(model => model.Description)
            .HasMaxLength(150)
            .IsRequired();

        //b.Property(model => model.DocumentContent)
        //    .IsRequired();


        b.Property(model => model.PublishedOn)
            .IsRequired();

        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);

        // Relationships
        b.HasOne(model => model.Website)
            .WithMany(model => model.Posts);
    }

    public static void ConfigureCategory(EntityTypeBuilder<Category> b)
    {
        // Fields
        b.HasKey(model => model.Id);

        b.Property(model => model.Name)
            .HasMaxLength(35)
            .IsRequired();

        b.Property(model => model.Slug)
            .HasMaxLength(35)
            .IsRequired();

        b.Property(model => model.Description)
            .HasMaxLength(200);

        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);
    }

    public static void ConfigureTag(EntityTypeBuilder<Tag> b)
    {
        // Fields
        b.HasKey(model => model.Id);
        b.Property(model => model.Name)
            .HasMaxLength(25)
            .IsRequired();
        b.Property(model => model.Slug)
            .HasMaxLength(25)
            .IsRequired();
        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);
    }

    public static void ConfigureEmail(EntityTypeBuilder<Email> b)
    {
        // Fields
        b.HasKey(model => model.Id);
        b.Property(model => model.Subject)
            .HasMaxLength(150)
            .IsRequired();
        b.Property(model => model.Message)
            .HasMaxLength(500)
            .IsRequired();
        b.Property(model => model.VanityId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()");

        AddPropertiesForAuditing(b);
    }

    // Many to many

    public static void ConfigurePostCategory(EntityTypeBuilder<PostCategory> b)
    {
        b.HasKey(model => new { model.PostId, model.CategoryId });
    }

    public static void ConfigurePostTag(EntityTypeBuilder<PostTag> b)
    {
        b.HasKey(model => new { model.PostId, model.TagId });

        b.HasOne(model => model.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(model => model.PostId);

        b.HasOne(model => model.Tag)
            .WithMany(c => c.PostTags)
            .HasForeignKey(model => model.TagId);
    }

    public static void ConfigurePostApplicationUser(EntityTypeBuilder<PostApplicationUser> b)
    {
        b.HasKey(model => new { model.PostId, model.ApplicationUserId });

        b.HasOne(model => model.Post)
            .WithMany(p => p.PostApplicationUsers)
            .HasForeignKey(model => model.PostId);

        b.HasOne(model => model.ApplicationUser)
            .WithMany(c => c.PostApplicationUsers)
            .HasForeignKey(model => model.ApplicationUserId);
    }

    public static void ConfigurePageApplicationUser(EntityTypeBuilder<PageApplicationUser> b)
    {
        b.HasKey(model => new { model.PageId, model.ApplicationUserId });

        b.HasOne(model => model.Page)
            .WithMany(p => p.PageApplicationUsers)
            .HasForeignKey(model => model.PageId);

        b.HasOne(model => model.ApplicationUser)
            .WithMany(c => c.PageApplicationUsers)
            .HasForeignKey(model => model.ApplicationUserId);
    }

    private static void AddPropertiesForAuditing<T>(EntityTypeBuilder<T> b) where T : BaseEntity
    {
        b.Property<DateTime>("DateCreated");
        b.Property<DateTime>("DateModified");
        b.Property<string>("UserCreated").HasMaxLength(50);
        b.Property<string>("UserModified").HasMaxLength(50);
    }
}
