﻿using System.ComponentModel.DataAnnotations;

namespace CmsEngine.Data.EditModels
{
    public class CategoryEditModel : BaseEditModel, IEditModel
    {
        [Required]
        [MaxLength(15, ErrorMessage = "The name must have less than 15 characters")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }
    }
}
