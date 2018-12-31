using System.Collections.Generic;

namespace CmsEngine.Data.Models
{
    public class Category : BaseModel
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }

        #endregion

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}
