using System;
using System.Collections.Generic;

namespace CmsEngine.Data.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string DocumentContent { get; set; }
        public string Author { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime PublishedOn { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }
}


