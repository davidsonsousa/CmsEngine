using Newtonsoft.Json.Linq;

namespace CmsEngine.Data.Entities
{
    public class Email : BaseEntity
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            var jsonResult = new JObject(
                                         new JProperty("From", From),
                                         new JProperty("Subject", Subject),
                                         new JProperty("Message", Message)
                                    );
            return jsonResult.ToString();
        }
    }
}
