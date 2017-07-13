﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CmsEngine.Attributes;

namespace CmsEngine.Data.Models
{
    public class Tag : BaseModel
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public Website Website { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }

        #endregion

        [Searchable, Orderable, ShowOnDataTable(0)]
        [Required]
        [MaxLength(15, ErrorMessage = "The name must have less than 15 characters")]
        public string Name { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Slug { get; set; }
    }
}