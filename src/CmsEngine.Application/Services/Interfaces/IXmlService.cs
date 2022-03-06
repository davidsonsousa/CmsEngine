namespace CmsEngine.Application.Services.Interfaces;

public interface IXmlService
{
    Task<XDocument> GenerateFeed();
    Task<XDocument> GenerateSitemap();
}
