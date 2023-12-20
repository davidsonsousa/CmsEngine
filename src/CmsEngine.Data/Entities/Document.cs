namespace CmsEngine.Data.Entities;

public abstract class Document : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? HeaderImage { get; set; }

    public string Description { get; set; } = string.Empty;

    public string DocumentContent { get; set; } = string.Empty;

    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

    public DateTime PublishedOn { get; set; } = DateTime.Now;

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId,
            [nameof(Title)] = Title,
            [nameof(Slug)] = Slug,
            [nameof(HeaderImage)] = HeaderImage,
            [nameof(Description)] = Description,
            [nameof(Status)] = Status.ToString(),
            [nameof(PublishedOn)] = PublishedOn
        };
        return jsonResult.ToString();
    }
}
