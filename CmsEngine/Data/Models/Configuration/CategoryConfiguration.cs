namespace CmsEngine.Data.Models.Configuration
{
    public class CategoryConfiguration : BaseConfiguration<Category>
    {
        public CategoryConfiguration() : base()
        {
            // Fields
            Property(category => category.Name).HasMaxLength(25).IsRequired();
            Property(category => category.Slug).HasMaxLength(25).IsRequired();
            Property(category => category.Description).HasMaxLength(200);
        }
    }
}
