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
        var jsonResult = new JObject(
                                    new JProperty("Id", Id),
                                    new JProperty("VanityId", VanityId),
                                    new JProperty("Title", Title),
                                    new JProperty("Slug", Slug),
                                    new JProperty("HeaderImage", HeaderImage),
                                    new JProperty("Description", Description),
                                    new JProperty("Status", Status.ToString()),
                                    new JProperty("PublishedOn", PublishedOn)
                                );
        return jsonResult.ToString();
    }
}
