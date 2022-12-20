namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class WebsiteTableViewModel : BaseViewModel, IViewModel
{
    [Searchable, Orderable, ShowOnDataTable(0)]
    public string Name { get; set; } = string.Empty;

    [Searchable, Orderable, ShowOnDataTable(1)]
    public string? Tagline { get; set; }

    [Searchable, Orderable, ShowOnDataTable(2)]
    public string? Culture { get; set; }

    [Searchable, Orderable, ShowOnDataTable(3)]
    public string UrlFormat { get; set; } = $"{Main.SiteUrl}/{Main.Type}/{Main.Slug}";

    [Searchable, Orderable, ShowOnDataTable(4)]
    public string DateFormat { get; set; } = "MM/dd/yyyy";

    [Searchable, Orderable, ShowOnDataTable(5)]
    public string? SiteUrl { get; set; }

    public string? GoogleAnalytics { get; set; }

    [Orderable, ShowOnDataTable(6)]
    public bool HasGoogleAnalytics {
        get {
            return !string.IsNullOrWhiteSpace(GoogleAnalytics);
        }
    }
}
