using System.Threading.Tasks;

namespace CmsEngine.Application.Helpers.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(ContactForm mailEditModel);
    }
}
