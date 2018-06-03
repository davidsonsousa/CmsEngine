using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsEngine.Data.Models
{
    public class BaseModel : IModel
    {
        #region Not mapped
        [NotMapped]
        public bool IsNew
        {
            get
            {
                return (Id == 0 && VanityId == Guid.Empty);
            }
        }
        #endregion

        public bool IsDeleted { get; set; }

        [Key]
        public int Id { get; set; }

        public Guid VanityId { get; set; }

        public string Slug { get; set; }

        #region For "quick log" purposes

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        #endregion
    }
}
