namespace CmsEngine.Application.Models.ViewModels;

public class InstanceViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Tagline { get; set; }

    public string? Description { get; set; }

    public string? HeaderImage { get; set; }

    public string Culture { get; set; } = string.Empty;

    public string UrlFormat { get; set; } = string.Empty;

    public string DateFormat { get; set; } = string.Empty;

    public string SiteUrl { get; set; } = string.Empty;

    public int ArticleLimit { get; set; }

    public ContactDetailsViewModel? ContactDetails { get; set; }
    public ApiDetailsViewModel? ApiDetails { get; set; }
    public DocumentViewModel? SelectedDocument { get; set; }
    public ContactForm? ContactForm { get; set; }
    public SocialMediaViewModel? SocialMedia { get; set; }
    public GoogleViewModel? Google { get; set; }

    public string PageTitle { get; set; } = string.Empty;

    public PaginatedList<PostViewModel>? PagedPosts { get; set; }
    public IEnumerable<PostViewModel>? LatestPosts { get; set; } // TODO: Maybe I need 2 versions of this property - For Index and for Footer
    public IEnumerable<PageViewModel>? Pages { get; set; }
    public IEnumerable<CategoryViewModel>? Categories { get; set; }
    public IEnumerable<CategoryViewModel>? CategoriesWithPosts { get; set; }
    public IEnumerable<TagViewModel>? Tags { get; set; }
}
