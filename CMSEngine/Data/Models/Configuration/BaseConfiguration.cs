using System.Data.Entity.ModelConfiguration;

namespace CmsEngine.Data.Models.Configuration
{
    public class BaseConfiguration<T> : EntityTypeConfiguration<T> where T : BaseModel
    {
        public BaseConfiguration()
        {
            Property(x => x.UserCreated).HasMaxLength(20);
            Property(x => x.UserModified).HasMaxLength(20);
        }
    }
}
