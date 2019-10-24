using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsEngine.Data.Entities
{
    public class Post : Document
    {
        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; }

        // Properties used for data projection only
        [NotMapped]
        public IEnumerable<Category> Categories { get; set; }
        [NotMapped]
        public IEnumerable<Tag> Tags { get; set; }
        [NotMapped]
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}
