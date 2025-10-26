namespace CmsEngine.Data.Repositories;

public class WebsiteRepository : Repository<Website>, IWebsiteRepository
{
    public WebsiteRepository(CmsEngineContext context) : base(context)
    {

    }

    public IQueryable<Website> GetForDataTable()
    {
        return Get().Select(w => new Website
        {
            VanityId = w.VanityId,
            Name = w.Name,
            Tagline = w.Tagline,
            Culture = w.Culture,
            UrlFormat = w.UrlFormat,
            DateFormat = w.DateFormat,
            SiteUrl = w.SiteUrl,
            GoogleAnalytics = w.GoogleAnalytics
        });
    }

    public Website? GetWebsiteInstanceByHost(string host)
    {
        return Get(q => q.SiteUrl == host).SingleOrDefault();
    }
}
