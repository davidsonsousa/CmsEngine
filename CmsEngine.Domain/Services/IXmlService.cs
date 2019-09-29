using System.Threading.Tasks;
using System.Xml.Linq;

namespace CmsEngine.Domain.Services
{
    public interface IXmlService
    {
        Task<XDocument> GenerateFeed();
        Task<XDocument> GenerateSitemap();
    }
}
