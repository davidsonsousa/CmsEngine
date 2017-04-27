namespace CmsEngine.Data.Models.Configuration
{
    public class WebsiteConfiguration : BaseConfiguration<Website>
    {
        public WebsiteConfiguration() : base()
        {
            // Fields
            Property(website => website.Name).HasMaxLength(25).IsRequired();
            Property(website => website.Description).HasMaxLength(200);
            Property(website => website.Culture).HasMaxLength(5).IsRequired();
            Property(website => website.UrlFormat).HasMaxLength(100).IsRequired();
            Property(website => website.DateFormat).HasMaxLength(10).IsRequired();
            Property(website => website.SiteUrl).HasMaxLength(250).IsRequired();

            // Relationships
            HasMany(website => website.Posts);
            HasMany(website => website.Pages);
            HasMany(website => website.Tags);
            HasMany(website => website.Categories);
        }
    }
}
