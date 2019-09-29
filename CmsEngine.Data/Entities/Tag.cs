using System.Collections.Generic;

namespace CmsEngine.Data.Entities
{
    public class Tag : BaseEntity
    {
        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
