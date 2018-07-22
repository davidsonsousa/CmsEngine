using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CmsEngine.Data.Models
{
    public class Category : BaseModel
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }

        #endregion

        [Required]
        [MaxLength(15, ErrorMessage = "The name must have less than 15 characters")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }
    }
}
