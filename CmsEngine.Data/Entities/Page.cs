using System.Collections.Generic;

namespace CmsEngine.Data.Entities
{
    public class Page : Document
    {
        public int WebsiteId { get; set; }

        public virtual Website Website { get; set; }

        public virtual ICollection<PageApplicationUser> PageApplicationUsers { get; set; }
    }
}
