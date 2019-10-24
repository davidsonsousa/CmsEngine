using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsEngine.Data.Entities
{
    public class Page : Document
    {
        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }
        public virtual ICollection<PageApplicationUser> PageApplicationUsers { get; set; }

        // Property used for data projection only
        [NotMapped]
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

    }
}
