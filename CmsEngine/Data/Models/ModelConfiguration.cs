using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CmsEngine.Data.Models
{
    public static class ModelConfiguration
    {
        public static void ConfigureWebsite(EntityTypeBuilder<Website> b)
        {
            // Fields
            b.HasKey(website => website.Id);
            b.Property(website => website.Name)
                .HasMaxLength(200)
                .IsRequired();
            b.Property(website => website.Description)
                .HasMaxLength(200);
            b.Property(website => website.Culture)
                .HasMaxLength(5)
                .IsRequired();
            b.Property(website => website.UrlFormat)
                .HasMaxLength(100)
                .IsRequired();
            b.Property(website => website.DateFormat)
                .HasMaxLength(10)
                .IsRequired();
            b.Property(website => website.SiteUrl)
                .HasMaxLength(250)
                .IsRequired();
            b.Property(website => website.UserCreated)
                .HasMaxLength(20);
            b.Property(website => website.UserModified)
                .HasMaxLength(20);

            // Relationships
            b.HasMany(website => website.Posts);
            b.HasMany(website => website.Pages);
            b.HasMany(website => website.Tags);
            b.HasMany(website => website.Categories);
        }

        public static void ConfigurePage(EntityTypeBuilder<Page> b)
        {
            // Fields
            b.HasKey(page => page.Id);
            b.Property(page => page.Title)
                .HasMaxLength(100)
                .IsRequired();
            b.Property(page => page.Slug)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(page => page.Description)
                .HasMaxLength(150)
                .IsRequired();
            b.Property(page => page.DocumentContent)
                .IsRequired();
            b.Property(page => page.Author)
                .HasMaxLength(20)
                .IsRequired();
            b.Property(page => page.PublishedOn)
                .IsRequired();
            b.Property(page => page.UserCreated)
                .HasMaxLength(20);
            b.Property(page => page.UserModified)
                .HasMaxLength(20);
            b.Property(page => page.UserCreated)
                .HasMaxLength(20);
            b.Property(page => page.UserModified)
                .HasMaxLength(20);

            // Relationships
            b.HasOne(page => page.Website)
                .WithMany(website => website.Pages);
        }

        public static void ConfigurePost(EntityTypeBuilder<Post> b)
        {
            // Fields
            b.HasKey(post => post.Id);
            b.Property(post => post.Title)
                .HasMaxLength(100)
                .IsRequired();
            b.Property(post => post.Slug)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(post => post.Description)
                .HasMaxLength(150)
                .IsRequired();
            b.Property(post => post.DocumentContent)
                .IsRequired();
            b.Property(post => post.Author)
                .HasMaxLength(20)
                .IsRequired();
            b.Property(post => post.PublishedOn)
                .IsRequired();
            b.Property(post => post.UserCreated)
                .HasMaxLength(20);
            b.Property(post => post.UserModified)
                .HasMaxLength(20);
            b.Property(post => post.UserCreated)
                .HasMaxLength(20);
            b.Property(post => post.UserModified)
                .HasMaxLength(20);

            // Relationships
            b.HasOne(post => post.Website)
                .WithMany(website => website.Posts);
        }

        public static void ConfigureCategory(EntityTypeBuilder<Category> b)
        {
            // Fields
            b.HasKey(category => category.Id);
            b.Property(category => category.Name)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(category => category.Slug)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(category => category.Description)
                .HasMaxLength(200);
            b.Property(category => category.UserCreated)
                .HasMaxLength(20);
            b.Property(category => category.UserModified)
                .HasMaxLength(20);
        }

        public static void ConfigureTag(EntityTypeBuilder<Tag> b)
        {
            // Fields
            b.HasKey(tag => tag.Id);
            b.Property(tag => tag.Name)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(tag => tag.Slug)
                .HasMaxLength(25)
                .IsRequired();
            b.Property(tag => tag.UserCreated)
                .HasMaxLength(20);
            b.Property(tag => tag.UserModified)
                .HasMaxLength(20);
        }

        // Many to many

        public static void ConfigurePostCategory(EntityTypeBuilder<PostCategory> b)
        {
            b.HasKey(pc => new { pc.PostId, pc.CategoryId });

            b.HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostId);

            b.HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }

        public static void ConfigurePostTag(EntityTypeBuilder<PostTag> b)
        {
            b.HasKey(pt => new { pt.PostId, pt.TagId });

            b.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            b.HasOne(pt => pt.Tag)
                .WithMany(c => c.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}
