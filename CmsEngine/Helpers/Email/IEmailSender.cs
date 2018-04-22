using System.Threading.Tasks;

namespace CmsEngine.Helpers.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
