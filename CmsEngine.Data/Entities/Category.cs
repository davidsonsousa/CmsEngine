using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace CmsEngine.Data.Entities
{
    public class Category : BaseEntity
    {
        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        // Property for data projection only
        [NotMapped]
        public int PostCount { get; set; }

        public override string ToString()
        {
            var jsonResult = new JObject(
                                        new JProperty("Id", Id),
                                        new JProperty("VanityId", VanityId),
                                        new JProperty("Name", Name),
                                        new JProperty("Slug", Slug),
                                        new JProperty("Description", Description)
                                    );
            return jsonResult.ToString();
        }
    }
}
