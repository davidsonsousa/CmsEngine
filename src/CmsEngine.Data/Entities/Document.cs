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
        // Provide a concise, low-allocation representation used for logs/debug only
        return $"{GetType().Name}(Id={Id},VanityId={VanityId},Title={Title})";
    }
}
