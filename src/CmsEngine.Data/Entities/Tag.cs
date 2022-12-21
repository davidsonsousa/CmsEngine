namespace CmsEngine.Data.Entities;

public class Tag : BaseEntity
{
    public int WebsiteId { get; set; }

    public virtual Website Website { get; set; } = null!;

    public virtual ICollection<PostTag>? PostTags { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;
}
