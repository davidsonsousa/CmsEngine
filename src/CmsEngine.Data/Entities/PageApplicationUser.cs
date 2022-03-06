namespace CmsEngine.Data.Entities;

[Table("PageAspNetUser")]
public class PageApplicationUser
{
    public int PageId { get; set; }
    public virtual Page Page { get; set; }

    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}
