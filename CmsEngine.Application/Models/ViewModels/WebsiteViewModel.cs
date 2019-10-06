using CmsEngine.Core;

namespace CmsEngine.Application.ViewModels
{
    public class WebsiteViewModel : BaseViewModel, IViewModel
    {
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string HeaderImagePath { get; set; }
        public string Culture { get; set; }
        public string UrlFormat { get; set; } = $"{Constants.SiteUrl}/{Constants.Type}/{Constants.Slug}";
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string SiteUrl { get; set; }
    }
}