using System.ComponentModel.DataAnnotations;
using CmsEngine.Core.Constants;

namespace CmsEngine.Application.EditModels
{
    public class WebsiteEditModel : BaseEditModel, IEditModel
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Tagline { get; set; }

        public string Description { get; set; }

        public string HeaderImage { get; set; }

        [Required]
        [MaxLength(5)]
        public string Culture { get; set; }

        [Required]
        [MaxLength(100)]
        public string UrlFormat { get; set; } = $"{CmsEngineConstants.SiteUrl}/{CmsEngineConstants.Type}/{CmsEngineConstants.Slug}";

        [Required]
        [MaxLength(10)]
        public string DateFormat { get; set; } = "MM/dd/yyyy";

        [Required]
        [MaxLength(250)]
        public string SiteUrl { get; set; }

        [Required]
        public int ArticleLimit { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Facebook { get; set; }

        [MaxLength(20)]
        public string Twitter { get; set; }

        [MaxLength(20)]
        public string Instagram { get; set; }

        [MaxLength(20)]
        public string LinkedIn { get; set; }

        [MaxLength(30)]
        public string FacebookAppId { get; set; }

        [MaxLength(10)]
        public string FacebookApiVersion { get; set; }

        [MaxLength(30)]
        public string DisqusShortName { get; set; }

        public string GoogleAnalytics { get; set; }
        public string GoogleRecaptchaSiteKey { get; set; }
        public string GoogleRecaptchaSecretKey { get; set; }
    }
}
