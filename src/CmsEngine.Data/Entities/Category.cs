namespace CmsEngine.Data.Entities;

public class Category : BaseEntity
{
    public int? WebsiteId { get; set; }
    public virtual Website? Website { get; set; }

    public virtual ICollection<PostCategory>? PostCategories { get; set; }

    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }

    // Properties for data projection only
    [NotMapped]
    public IEnumerable<Post> Posts { get; set; } = new List<Post>();

    [NotMapped]
    public int PostCount { get; set; }

    public override string ToString()
    {
        var jsonResult = new JObject(
                                    new JProperty("Id", Id),
                                    new JProperty("VanityId", VanityId),
                                    new JProperty("Name", Name),
                                    new JProperty("Slug", Slug),
                                    new JProperty("Description", Description)
                                );
        return jsonResult.ToString();
    }
}
