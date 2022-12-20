namespace CmsEngine.Data.Entities;

public class Post : Document
{
    public int WebsiteId { get; set; }

    public virtual Website Website { get; set; } = null!;

    public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; } = new List<PostApplicationUser>();


    // Properties used for data projection only
    [NotMapped]
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    [NotMapped]
    public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();

    [NotMapped]
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}
