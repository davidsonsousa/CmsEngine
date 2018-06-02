using System.Collections.Generic;

namespace CmsEngine.Data.ViewModels
{
    public class BlogViewModel
    {
        public IEnumerable<PostViewModel> PagedPosts { get; set; }
        public IEnumerable<PostViewModel> LatestPosts { get; set; }
        public IEnumerable<PageViewModel> Pages { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }

        public WebsiteViewModel Instance { get; set; }
    }
}
