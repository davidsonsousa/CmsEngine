using System.Collections.Generic;

namespace CmsEngine.Data.Models
{
    public class Post : Document
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        #endregion

    }
}
