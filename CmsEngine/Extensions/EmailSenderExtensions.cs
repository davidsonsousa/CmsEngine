using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CmsEngine.Helpers.Email;

namespace CmsEngine.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            var contactForm = new ContactForm
            {
                Sender = email,
                Subject = "Confirm your email",
                Message = $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>"
            };

            return emailSender.SendEmailAsync(contactForm);
        }

        public static Task SendPasswordResetAsync(this IEmailSender emailSender, string email, string link)
        {
            var contactForm = new ContactForm
            {
                Sender = email,
                Subject = "Reset Password",
                Message = $"Please reset your password by clicking here: <a href='{link}'>link</a>"
            };

            return emailSender.SendEmailAsync(contactForm);
        }
    }
}
