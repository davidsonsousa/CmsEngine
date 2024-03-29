namespace CmsEngine.Data.Entities;

public class Category : BaseEntity
{
    public int? WebsiteId { get; set; }

    public virtual Website? Website { get; set; }

    public virtual ICollection<PostCategory> PostCategories { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Properties for data projection only
    [NotMapped]
    public IEnumerable<Post> Posts { get; set; } = new List<Post>();

    [NotMapped]
    public int PostCount { get; set; }

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId,
            [nameof(Name)] = Name,
            [nameof(Slug)] = Slug,
            [nameof(Description)] = Description
        };
        return jsonResult.ToString();
    }
}
