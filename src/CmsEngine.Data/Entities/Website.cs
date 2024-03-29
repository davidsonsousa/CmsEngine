namespace CmsEngine.Data.Entities;

public class Website : BaseEntity
{
    public virtual ICollection<Page> Pages { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string? Tagline { get; set; }

    public string? Description { get; set; }

    public string? HeaderImage { get; set; }

    public string Culture { get; set; } = string.Empty;

    public string UrlFormat { get; set; } = $"{Main.SiteUrl}/{Main.Type}/{Main.Slug}";

    public string DateFormat { get; set; } = "MM/dd/yyyy";

    public string SiteUrl { get; set; } = string.Empty;

    public int ArticleLimit { get; set; }

    // Contact details
    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? LinkedIn { get; set; }

    // Api configuration
    public string? FacebookAppId { get; set; }

    public string? FacebookApiVersion { get; set; }

    public string? DisqusShortName { get; set; }

    public string? GoogleAnalytics { get; set; }

    public string? GoogleRecaptchaSiteKey { get; set; }

    public string? GoogleRecaptchaSecretKey { get; set; }

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId,
            [nameof(Name)] = Name,
            [nameof(SiteUrl)] = SiteUrl,
            [nameof(Tagline)] = Tagline
        };
        return jsonResult.ToString();
    }
}
