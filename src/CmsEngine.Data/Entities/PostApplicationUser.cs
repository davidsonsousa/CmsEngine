namespace CmsEngine.Data.Entities;

[Table("PostAspNetUser")]
public class PostApplicationUser
{
    public int PostId { get; set; }

    public virtual Post? Post { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    public virtual ApplicationUser? ApplicationUser { get; set; }
}
