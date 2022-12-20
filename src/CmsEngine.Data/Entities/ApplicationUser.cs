namespace CmsEngine.Data.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; }

    public virtual ICollection<PageApplicationUser> PageApplicationUsers { get; set; }
}
