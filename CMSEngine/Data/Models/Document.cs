using System;
using System.ComponentModel.DataAnnotations;
using CmsEngine.Attributes;

namespace CmsEngine.Data.Models
{
    public abstract class Document : BaseModel
    {
        public Document()
        {
            Status = DocumentStatus.Draft;
            PublishedOn = DateTime.Now;
        }

        [Searchable, Orderable, ShowOnDataTable(0)]
        [Required]
        [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
        public string Title { get; set; }

        [Searchable, Orderable, ShowOnDataTable(2)]
        public string Slug { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        [Required]
        [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
        public string Description { get; set; }

        public string DocumentContent { get; set; }

        [Searchable, Orderable, ShowOnDataTable(3)]
        public string Author { get; set; }

        [Searchable, Orderable, ShowOnDataTable(5)]
        public DocumentStatus Status { get; set; }

        [Searchable, Orderable, ShowOnDataTable(4)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PublishedOn { get; set; }
    }
}
