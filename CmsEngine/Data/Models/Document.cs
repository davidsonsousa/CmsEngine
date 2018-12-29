using System;

namespace CmsEngine.Data.Models
{
    public abstract class Document : BaseModel
    {
        public Document()
        {
            Status = DocumentStatus.Draft;
            PublishedOn = DateTime.Now;
        }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string HeaderImagePath { get; set; }
        public string HeaderImagePathThumb { get; set; }

        public string Description { get; set; }
        public string DocumentContent { get; set; }
        public DocumentStatus Status { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}
