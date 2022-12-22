namespace CmsEngine.Data.Entities;

public class Page : Document
{
    public int WebsiteId { get; set; }

    public virtual Website Website { get; set; } = null!;

    public virtual ICollection<PageApplicationUser> PageApplicationUsers { get; set; }

    // Property used for data projection only
    [NotMapped]
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

    public Page()
    {
        PageApplicationUsers = new List<PageApplicationUser>();

        ApplicationUsers = new List<ApplicationUser>();
    }
}
