using System.Threading.Tasks;

namespace CmsEngine.Domain.Helpers.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(ContactForm mailEditModel);
    }
}
