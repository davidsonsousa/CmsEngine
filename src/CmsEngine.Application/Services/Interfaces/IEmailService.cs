namespace CmsEngine.Application.Services.Interfaces;

public interface IEmailService
{
    Task<ReturnValue> Save(ContactForm contactForm);
    Task<IEnumerable<ContactForm>> GetOrderedByDate();
}
