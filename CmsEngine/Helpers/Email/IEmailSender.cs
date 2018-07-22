using System.Threading.Tasks;

namespace CmsEngine.Helpers.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(ContactForm mailEditModel);
    }
}
