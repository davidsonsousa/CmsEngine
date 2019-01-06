using System.Collections.Generic;

namespace CmsEngine.Data.Models
{
    public class Tag : BaseModel
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }

        #endregion

        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
