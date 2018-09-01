using System.ComponentModel.DataAnnotations.Schema;

namespace CmsEngine.Data.Models
{
    [Table("PostAspNetUser")]
    public class PostApplicationUser
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
