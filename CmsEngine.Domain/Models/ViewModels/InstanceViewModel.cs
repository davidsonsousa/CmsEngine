using System.Collections.Generic;
using CmsEngine.Core;
using CmsEngine.Domain.Helpers.Email;

namespace CmsEngine.Domain.ViewModels
{
    public class InstanceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public string Culture { get; set; }
        public string UrlFormat { get; set; }
        public string DateFormat { get; set; }
        public string SiteUrl { get; set; }
        public int ArticleLimit { get; set; }
        public ContactDetailsViewModel ContactDetails { get; set; }
        public ApiDetailsViewModel ApiDetails { get; set; }
        public DocumentViewModel SelectedDocument { get; set; }
        public ContactForm ContactForm { get; set; }
        public SocialMediaViewModel SocialMedia { get; set; }

        public string PageTitle { get; set; }

        public PaginatedList<PostViewModel> PagedPosts { get; set; }
        public IEnumerable<PostViewModel> LatestPosts { get; set; }
        public IEnumerable<PageViewModel> Pages { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}
