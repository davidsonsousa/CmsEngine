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

    public bool HasAddress {
        get {
            return ContactDetails is not null && !string.IsNullOrWhiteSpace(ContactDetails.Address);
        }
    }

    public bool HasPhone {
        get {
            return ContactDetails is not null && !string.IsNullOrWhiteSpace(ContactDetails.Phone);
        }
    }

    public bool HasEmail {
        get {
            return ContactDetails is not null && !string.IsNullOrWhiteSpace(ContactDetails.Email);
        }
    }

    public ApiDetailsViewModel? ApiDetails { get; set; }

    public bool HasFacebookDetails {
        get {
            return ApiDetails is not null
                   && !string.IsNullOrWhiteSpace(ApiDetails.FacebookAppId)
                   && !string.IsNullOrWhiteSpace(ApiDetails.FacebookApiVersion);
        }
    }

    public bool HasDisqusDetails {
        get {
            return ApiDetails is not null && !string.IsNullOrWhiteSpace(ApiDetails.DisqusShortName);
        }
    }

    public DocumentViewModel? SelectedDocument { get; set; }

    public ContactForm ContactForm { get; set; } = null!;

    public SocialMediaViewModel? SocialMedia { get; set; }

    public bool HasFacebook {
        get {
            return SocialMedia is not null && !string.IsNullOrWhiteSpace(SocialMedia.Facebook);
        }
    }

    public bool HasTwitter {
        get {
            return SocialMedia is not null && !string.IsNullOrWhiteSpace(SocialMedia.Twitter);
        }
    }

    public bool HasInstagram {
        get {
            return SocialMedia is not null && !string.IsNullOrWhiteSpace(SocialMedia.Instagram);
        }
    }

    public bool HasLinkedIn {
        get {
            return SocialMedia is not null && !string.IsNullOrWhiteSpace(SocialMedia.LinkedIn);
        }
    }

    public bool HasSocialMedia {
        get {
            return SocialMedia is not null && HasFacebook || HasTwitter || HasInstagram || HasLinkedIn;
        }
    }

    public GoogleViewModel? Google { get; set; }

    public bool HasGoogleAnalytics {
        get {
            return Google is not null && !string.IsNullOrWhiteSpace(Google.GoogleAnalytics);
        }
    }

    public bool HasRecaptchaSiteKey {
        get {
            return Google is not null && !string.IsNullOrWhiteSpace(Google.GoogleRecaptchaSiteKey);
        }
    }

    public bool HasRecaptchaSecretKey {
        get {
            return Google is not null && !string.IsNullOrWhiteSpace(Google.GoogleRecaptchaSecretKey);
        }
    }

    public string PageTitle { get; set; } = string.Empty;

    public PaginatedList<PostViewModel> PagedPosts { get; set; } = new PaginatedList<PostViewModel>();

    // TODO: Maybe I need 2 versions of this property - For Index and for Footer
    public IEnumerable<PostViewModel> LatestPosts { get; set; } = new List<PostViewModel>();

    public IEnumerable<PageViewModel> Pages { get; set; } = new List<PageViewModel>();

    public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    public IEnumerable<CategoryViewModel> CategoriesWithPosts { get; set; } = new List<CategoryViewModel>();

    public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
}
