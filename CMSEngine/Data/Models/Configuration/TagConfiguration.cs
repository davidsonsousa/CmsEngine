namespace CmsEngine.Data.Models.Configuration
{
    public class TagConfiguration: BaseConfiguration<Tag>
    {
        public TagConfiguration() : base()
        {
            // Fields
            Property(tag => tag.Name).HasMaxLength(25).IsRequired();
            Property(tag => tag.Slug).HasMaxLength(25).IsRequired();
        }
    }
}
