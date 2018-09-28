using System;

namespace CmsEngine.Data.ViewModels
{
    public class DocumentViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string HeaderImagePath { get; set; }
        public string DocumentContent { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime PublishedOn { get; set; }
    }
}
