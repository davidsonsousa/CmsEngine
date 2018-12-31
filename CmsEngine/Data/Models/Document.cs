using System;
using System.ComponentModel.DataAnnotations;

namespace CmsEngine.Data.Models
{
    public abstract class Document : BaseModel
    {
        public Document()
        {
            Status = DocumentStatus.Draft;
            PublishedOn = DateTime.Now;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
        public string Description { get; set; }

        public string DocumentContent { get; set; }

        public DocumentStatus Status { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PublishedOn { get; set; }
    }
}
