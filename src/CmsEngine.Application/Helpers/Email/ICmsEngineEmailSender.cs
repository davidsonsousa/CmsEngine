namespace CmsEngine.Application.Helpers.Email;

public interface ICmsEngineEmailSender
{
    Task SendEmailAsync(ContactForm mailEditModel);
}
