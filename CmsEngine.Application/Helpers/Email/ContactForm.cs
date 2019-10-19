using Newtonsoft.Json.Linq;

namespace CmsEngine.Application.Helpers.Email
{
    public class ContactForm
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Message { get; }

        public ContactForm(string from, string to, string subject, string message) : this(to, subject, message)
        {
            From = from;
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
                                        new JProperty("Subject", Subject),
                                        new JProperty("Message", Message)
                                    );
            return jsonResult.ToString();
        }
    }
}
