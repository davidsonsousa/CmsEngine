namespace CmsEngine.Application.Models.ViewModels;

public class WebsiteViewModel : BaseViewModel, IViewModel
{
    public string Name { get; set; } = string.Empty;

    public string? Tagline { get; set; }

    public string? Description { get; set; }

    public string? HeaderImagePath { get; set; }

    public string Culture { get; set; } = string.Empty;

    public string UrlFormat { get; set; } = $"{Main.SiteUrl}/{Main.Type}/{Main.Slug}";

    public string DateFormat { get; set; } = "MM/dd/yyyy";

    public string SiteUrl { get; set; } = string.Empty;
}
