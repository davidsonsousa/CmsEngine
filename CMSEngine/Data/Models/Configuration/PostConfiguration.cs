namespace CmsEngine.Data.Models.Configuration
{
    public class PostConfiguration : BaseConfiguration<Post>
    {
        public PostConfiguration() : base()
        {
            // Fields
            Property(post => post.Title).HasMaxLength(100).IsRequired();
            Property(post => post.Slug).HasMaxLength(25).IsRequired();
            Property(post => post.Description).HasMaxLength(150).IsRequired();
            Property(post => post.DocumentContent).IsRequired();
            Property(post => post.Author).HasMaxLength(20).IsRequired();
            Property(post => post.PublishedOn).IsRequired();
            Property(post => post.UserCreated).HasMaxLength(20);
            Property(post => post.UserModified).HasMaxLength(20);

            // Relationships
            HasRequired(post => post.Website).WithMany(website => website.Posts);
            HasMany(post => post.Categories);
            HasMany(post => post.Tags);
        }
    }
}
