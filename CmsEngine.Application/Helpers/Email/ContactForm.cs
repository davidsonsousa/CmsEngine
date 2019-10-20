using Newtonsoft.Json.Linq;

namespace CmsEngine.Application.Helpers.Email
{
    public class ContactForm
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public ContactForm()
        {

        }

        public ContactForm(string to, string subject, string message)
        {
            To = to;
            Subject = subject;
            Message = message;
        }

        public override string ToString()
        {
            var jsonResult = new JObject(
                                        new JProperty("From", From),
                                        new JProperty("To", To),
                                        new JProperty("Subject", Subject),
                                        new JProperty("Message", Message)
                                    );
            return jsonResult.ToString();
        }
    }
}
