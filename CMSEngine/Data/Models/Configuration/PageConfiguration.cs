namespace CmsEngine.Data.Models.Configuration
{
    public class PageConfiguration : BaseConfiguration<Page>
    {
        public PageConfiguration() : base()
        {
            // Fields
            Property(page => page.Title).HasMaxLength(100).IsRequired();
            Property(page => page.Slug).HasMaxLength(25).IsRequired();
            Property(page => page.Description).HasMaxLength(150).IsRequired();
            Property(page => page.DocumentContent).IsRequired();
            Property(page => page.Author).HasMaxLength(20).IsRequired();
            Property(page => page.PublishedOn).IsRequired();
            Property(page => page.UserCreated).HasMaxLength(20);
            Property(page => page.UserModified).HasMaxLength(20);

            // Relationships
            HasRequired(page => page.Website).WithMany(website => website.Pages);
        }
    }
}
