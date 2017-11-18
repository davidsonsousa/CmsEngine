using System;
using System.ComponentModel.DataAnnotations;

namespace CmsEngine.Data.EditModels
{
    public class PageEditModel : BaseEditModel, IEditModel
    {
        public PageEditModel()
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

        public string Author { get; set; }

        public DocumentStatus Status { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime PublishedOn { get; set; }

    }
}
