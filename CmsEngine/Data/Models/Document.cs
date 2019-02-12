using System;
using Newtonsoft.Json.Linq;

namespace CmsEngine.Data.Models
{
    public abstract class Document : BaseModel
    {
        public Document()
        {
            Status = DocumentStatus.Draft;
            PublishedOn = DateTime.Now;
        }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string HeaderImage { get; set; }

        public string Description { get; set; }
        public string DocumentContent { get; set; }
        public DocumentStatus Status { get; set; }

        public DateTime PublishedOn { get; set; }

        public override string ToString()
        {
            var jsonResult = new JObject(
                                        new JProperty("Id", Id),
                                        new JProperty("VanityId", VanityId),
                                        new JProperty("Title", Title),
                                        new JProperty("Slug", Slug),
                                        new JProperty("HeaderImage", HeaderImage),
                                        new JProperty("Description", Description),
                                        new JProperty("Status", Status.ToString()),
                                        new JProperty("PublishedOn", PublishedOn)
                                    );
            return jsonResult.ToString();
        }
    }
}
