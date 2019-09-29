using System.Collections.Generic;

namespace CmsEngine.Data.Entities
{
    public class Post : Document
    {
        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }

        public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; }
    }
}
