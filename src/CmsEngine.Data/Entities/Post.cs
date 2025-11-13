namespace CmsEngine.Data.Entities;

public class Post : Document
{
    public int WebsiteId { get; set; }

    public virtual Website Website { get; set; } = null!;

    private ICollection<PostCategory>? _postCategories;
    public virtual ICollection<PostCategory> PostCategories
    {
        get => _postCategories ??= new List<PostCategory>();
        set => _postCategories = value;
    }

    private ICollection<PostTag>? _postTags;
    public virtual ICollection<PostTag> PostTags
    {
        get => _postTags ??= new List<PostTag>();
        set => _postTags = value;
    }

    private ICollection<PostApplicationUser>? _postApplicationUsers;
    public virtual ICollection<PostApplicationUser> PostApplicationUsers
    {
        get => _postApplicationUsers ??= new List<PostApplicationUser>();
        set => _postApplicationUsers = value;
    }


    // Properties used for data projection only
    [NotMapped]
    public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

    [NotMapped]
    public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

    [NotMapped]
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = Enumerable.Empty<ApplicationUser>();
}
