namespace CmsEngine.Application.Extensions.Mapper;

public static class WebsiteExtensions
{
    /// <summary>
    /// Maps Website model into a WebsiteEditModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static WebsiteEditModel MapToEditModel(this Website item)
    {
        return new WebsiteEditModel
        {
            Id = item.Id,
            VanityId = item.VanityId,
            Name = item.Name,
            Description = item.Description,
            Tagline = item.Tagline,
            HeaderImage = item.HeaderImage,
            Culture = item.Culture,
            UrlFormat = item.UrlFormat,
            DateFormat = item.DateFormat,
            SiteUrl = item.SiteUrl,
            ArticleLimit = item.ArticleLimit,
            Address = item.Address,
            Phone = item.Phone,
            Email = item.Email,
            FacebookAppId = item.FacebookAppId,
            FacebookApiVersion = item.FacebookApiVersion,
            DisqusShortName = item.DisqusShortName,
            Facebook = item.Facebook,
            Twitter = item.Twitter,
            Instagram = item.Instagram,
            LinkedIn = item.LinkedIn,
            GoogleAnalytics = item.GoogleAnalytics,
            GoogleRecaptchaSiteKey = item.GoogleRecaptchaSiteKey,
            GoogleRecaptchaSecretKey = item.GoogleRecaptchaSecretKey
        };
    }

    /// <summary>
    /// Maps a WebsiteEditModel into a Website
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Website MapToModel(this WebsiteEditModel item)
    {
        return new Website
        {
            Id = item.Id,
            VanityId = item.VanityId,
            Name = item.Name,
            Description = item.Description,
            Tagline = item.Tagline,
            HeaderImage = item.HeaderImage,
            Culture = item.Culture,
            UrlFormat = item.UrlFormat,
            DateFormat = item.DateFormat,
            SiteUrl = item.SiteUrl,
            ArticleLimit = item.ArticleLimit,
            Address = item.Address,
            Phone = item.Phone,
            Email = item.Email,
            FacebookAppId = item.FacebookAppId,
            FacebookApiVersion = item.FacebookApiVersion,
            DisqusShortName = item.DisqusShortName,
            Facebook = item.Facebook,
            Twitter = item.Twitter,
            Instagram = item.Instagram,
            LinkedIn = item.LinkedIn,
            GoogleAnalytics = item.GoogleAnalytics,
            GoogleRecaptchaSiteKey = item.GoogleRecaptchaSiteKey,
            GoogleRecaptchaSecretKey = item.GoogleRecaptchaSecretKey
        };
    }

    /// <summary>
    /// Maps a WebsiteEditModel into a specific Website
    /// </summary>
    /// <param name="item"></param>
    /// <param name="website"></param>
    /// <returns></returns>
    public static Website MapToModel(this WebsiteEditModel item, Website website)
    {
        website.Id = item.Id;
        website.VanityId = item.VanityId;
        website.Name = item.Name;
        website.Description = item.Description;
        website.Tagline = item.Tagline;
        website.HeaderImage = item.HeaderImage;
        website.Culture = item.Culture;
        website.UrlFormat = item.UrlFormat;
        website.DateFormat = item.DateFormat;
        website.SiteUrl = item.SiteUrl;
        website.ArticleLimit = item.ArticleLimit;
        website.Address = item.Address;
        website.Phone = item.Phone;
        website.Email = item.Email;
        website.FacebookAppId = item.FacebookAppId;
        website.FacebookApiVersion = item.FacebookApiVersion;
        website.DisqusShortName = item.DisqusShortName;
        website.Facebook = item.Facebook;
        website.Twitter = item.Twitter;
        website.Instagram = item.Instagram;
        website.LinkedIn = item.LinkedIn;
        website.GoogleAnalytics = item.GoogleAnalytics;
        website.GoogleRecaptchaSiteKey = item.GoogleRecaptchaSiteKey;
        website.GoogleRecaptchaSecretKey = item.GoogleRecaptchaSecretKey;

        return website;
    }

    /// <summary>
    /// Maps an IEnumerable<Website> into an IEnumerable<WebsiteTableViewModel>
    /// </summary>
    /// <param name="websites"></param>
    /// <returns></returns>
    public static IEnumerable<WebsiteTableViewModel> MapToTableViewModel(this IEnumerable<Website> websites)
    {
        return websites.Select(item => new WebsiteTableViewModel
        {
            //Id = item.Id,
            VanityId = item.VanityId,
            Name = item.Name,
            Tagline = item.Tagline,
            Culture = item.Culture,
            UrlFormat = item.UrlFormat,
            DateFormat = item.DateFormat,
            SiteUrl = item.SiteUrl,
            GoogleAnalytics = item.GoogleAnalytics
        }).ToList();
    }
}
