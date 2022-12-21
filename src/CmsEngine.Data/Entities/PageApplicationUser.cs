namespace CmsEngine.Data.Entities;

[Table("PageAspNetUser")]
public class PageApplicationUser
{
    public int PageId { get; set; }

    public virtual Page Page { get; set; } = null!;

    public string ApplicationUserId { get; set; } = string.Empty;

    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
}
