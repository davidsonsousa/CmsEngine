using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace CmsEngine.Application.EditModels
{
    public class CategoryEditModel : BaseEditModel, IEditModel
    {
        [Required]
        [MaxLength(35, ErrorMessage = "The name must have less than 35 characters")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            var jsonResult = new JObject(
                                        new JProperty("Id", Id),
                                        new JProperty("VanityId", VanityId),
                                        new JProperty("Name", Name),
                                        new JProperty("Slug", Slug),
                                        new JProperty("Description", Description)
                                    );
            return jsonResult.ToString();
        }
    }
}
