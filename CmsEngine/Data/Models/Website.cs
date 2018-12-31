using System.Collections.Generic;

namespace CmsEngine.Data.Models
{
    public class Website : BaseModel
    {
        #region Navigation

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

        #endregion

        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public string Culture { get; set; }
        public string UrlFormat { get; set; } = $"{Constants.SiteUrl}/{Constants.Type}/{Constants.Slug}";
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string SiteUrl { get; set; }
        public int ArticleLimit { get; set; }

        // Contact details
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }

        // Api configuration
        public string FacebookAppId { get; set; }
        public string FacebookApiVersion { get; set; }
    }
}
