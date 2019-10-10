using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using CmsEngine.Application.ViewModels;
using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class XmlService : Service, IXmlService
    {
        private readonly IUnitOfWork _unitOfWork;

        public XmlService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                         : base(uow, hca, loggerFactory, memoryCache)
        {
            _unitOfWork = uow;
        }

        public async Task<XDocument> GenerateFeed()
        {
            var articleList = new List<XElement>();

            foreach (var item in await _unitOfWork.Posts.GetPublishedPostsOrderByDescending(o => o.PublishedOn))
            {
                string url = FormatUrl("feed", item.Slug);
                articleList.Add(new XElement("item",
                                          new XElement("title", item.Title),
                                          new XElement("link", url),
                                          new XElement("description", item.DocumentContent),
                                          new XElement("pubDate", item.PublishedOn.ToString("r")),
                                          new XElement("guid", url)));
            }

            return new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("rss",
                             new XElement("channel",
                                          new XElement("title", Instance.Name),
                                          new XElement("link", FormatUrl(string.Empty)),
                                          new XElement("description", Instance.Description),
                                          new XElement("language", Instance.Culture.ToLowerInvariant()),
                                          new XElement("generator", "MultiCMS"),
                                          articleList
                                          ),
                             new XAttribute("version", "2.0")));
        }

        public async Task<XDocument> GenerateSitemap()
        {
            var items = new List<SitemapViewModel>();

            var orderedPosts = await _unitOfWork.Posts.GetPublishedPostsOrderByDescending(o => o.PublishedOn);
            items.AddRange(orderedPosts.Select(x => new SitemapViewModel
            {
                Url = FormatUrl("post", x.Slug),
                PublishedOn = x.PublishedOn.ToString("yyyy-MM-dd")
            }));

            var orderedPages = await _unitOfWork.Pages.GetOrderByDescending(o => o.PublishedOn);
            items.AddRange(orderedPages.Select(x => new SitemapViewModel
            {
                Url = FormatUrl("post", x.Slug),
                PublishedOn = x.PublishedOn.ToString("yyyy-MM-dd")
            }));

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            return new XDocument(new XDeclaration("1.0", "utf-8", null),
                                         new XElement(ns + "urlset",
                                                      from item in items
                                                      select new XElement(ns + "url",
                                                                new XElement(ns + "loc", item.Url),
                                                                new XElement(ns + "lastmod", item.PublishedOn),
                                                                new XElement(ns + "changefreq", "monthly")
                                                             )));

        }

        private string FormatUrl(string type, string slug = "")
        {
            string url = "";

            if (!string.IsNullOrWhiteSpace(Instance.UrlFormat))
            {
                url = Instance.UrlFormat.Replace("[site_url]", Instance.SiteUrl)
                                                  .Replace("[culture]", Instance.Culture)
                                                  .Replace("[short_culture]", Instance.Culture.Substring(0, 2))
                                                  .Replace("[type]", type)
                                                  .Replace("[slug]", slug);
            }

            url = url.EndsWith("/") ? url.Substring(0, url.LastIndexOf('/')) : url;

            return url;
        }
    }
}
