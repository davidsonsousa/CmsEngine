using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

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

        #region For "quick log" purposes

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        #endregion

        public override string ToString()
        {
            var jsonResult = new JObject(
                                        new JProperty("Id", Id),
                                        new JProperty("VanityId", VanityId)
                                    );
            return jsonResult.ToString();
        }
    }
}
