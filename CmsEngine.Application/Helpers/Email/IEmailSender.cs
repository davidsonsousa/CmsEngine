using System.Threading.Tasks;

namespace CmsEngine.Application.Helpers.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(ContactForm mailEditModel);
        Task SendPasswordResetAsync(ContactForm mailEditModel);
        Task SendEmailConfirmationAsync(ContactForm mailEditModel);
    }
}
