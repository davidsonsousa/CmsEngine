namespace CmsEngine.Application.Models.ViewModels;

public class DocumentViewModel : BaseViewModel
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string HeaderImage { get; set; }
    public string DocumentContent { get; set; }
    public DocumentStatus Status { get; set; }
    public string PublishedOn { get; set; }
}
