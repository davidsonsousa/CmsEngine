namespace CmsEngine.Application.Models.ViewModels;

public class DocumentViewModel : BaseViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? HeaderImage { get; set; }

    public string DocumentContent { get; set; } = string.Empty;

    public DocumentStatus Status { get; set; }

    public string PublishedOn { get; set; } = string.Empty;
}
