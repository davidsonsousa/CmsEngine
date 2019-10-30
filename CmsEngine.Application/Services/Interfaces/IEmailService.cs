using System.Collections.Generic;
using System.Threading.Tasks;
using CmsEngine.Application.Helpers.Email;
using CmsEngine.Core;

namespace CmsEngine.Application.Services
{
    public interface IEmailService
    {
        Task<ReturnValue> Save(ContactForm contactForm);
        Task<IEnumerable<ContactForm>> GetOrderedByDate();
    }
}
