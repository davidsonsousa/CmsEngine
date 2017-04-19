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
                return (this.Id == 0);
            }
        }
        #endregion

        public bool IsDeleted { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VanityId { get; set; }

        #region For "quick log" purposes

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        #endregion
    }
}
