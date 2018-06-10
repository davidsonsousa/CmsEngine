namespace CmsEngine.Data.ViewModels
{
    public class WebsiteViewModel : BaseViewModel, IViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Culture { get; set; }

        public string UrlFormat { get; set; } = $"{Constants.SiteUrl}/{Constants.Type}/{Constants.Slug}";

        public string DateFormat { get; set; } = "MM/dd/yyyy";

        public string SiteUrl { get; set; }
    }
}
