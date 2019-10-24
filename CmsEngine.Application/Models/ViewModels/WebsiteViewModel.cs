using CmsEngine.Core.Constants;

namespace CmsEngine.Application.ViewModels
{
    public class WebsiteViewModel : BaseViewModel, IViewModel
    {
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string HeaderImagePath { get; set; }
        public string Culture { get; set; }
        public string UrlFormat { get; set; } = $"{CmsEngineConstants.SiteUrl}/{CmsEngineConstants.Type}/{CmsEngineConstants.Slug}";
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string SiteUrl { get; set; }
    }
}
