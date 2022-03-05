namespace CmsEngine.Data.Entities;

public class Post : Document
{
    public int WebsiteId { get; set; }
    public virtual Website Website { get; set; }
    public virtual ICollection<PostCategory> PostCategories { get; set; }
    public virtual ICollection<PostTag> PostTags { get; set; }
    public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; }

    // Properties used for data projection only
    [NotMapped]
    public IEnumerable<Category> Categories { get; set; }
    [NotMapped]
    public IEnumerable<Tag> Tags { get; set; }
    [NotMapped]
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

    public Post()
    {
        PostCategories = new List<PostCategory>();
        PostTags = new List<PostTag>();
        PostApplicationUsers = new List<PostApplicationUser>();

        Categories = new List<Category>();
        Tags = new List<Tag>();
        ApplicationUsers = new List<ApplicationUser>();
    }
}
