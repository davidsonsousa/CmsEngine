using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSEngine.Data.Models
{
    public class BaseModel
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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VanityId { get; set; }

        public bool IsDeleted { get; set; }

        #region For "quick log" purposes

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        #endregion
    }
}
