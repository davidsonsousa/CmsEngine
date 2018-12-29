using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsEngine.Data.EditModels
{
    public class PostEditModel : BaseEditModel, IEditModel
    {
        public PostEditModel()
        {
            Status = DocumentStatus.Draft;
            PublishedOn = DateTime.Now;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
        public string Title { get; set; }

        public string Slug { get; set; }

        public string HeaderImagePath { get; set; }
        public string HeaderImagePathThumb { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
        public string Description { get; set; }

        public string DocumentContent { get; set; }

        public IEnumerable<CheckboxEditModel> Categories { get; set; }

        public IEnumerable<string> SelectedCategories { get; set; }

        // TODO: Perhaps replace the SelectListItem by something else in order to make it less ASP.NET Core dependent
        public IEnumerable<SelectListItem> Tags { get; set; }

        public IEnumerable<string> SelectedTags { get; set; }

        public DocumentStatus Status { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PublishedOn { get; set; }

    }
}
