namespace CmsEngine.Application.Helpers.Email
{
    public class ContactForm
    {
        public string Sender { get; }
        public string Subject { get; }
        public string Message { get; }

        public ContactForm(string sender, string subject, string message)
        {
            Sender = sender;
            Subject = subject;
            Message = message;
        }
    }
}
