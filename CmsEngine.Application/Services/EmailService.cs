using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Extensions.Mapper;
using CmsEngine.Application.Helpers.Email;
using CmsEngine.Core;
using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class EmailService : Service, IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmailService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                              : base(uow, hca, loggerFactory, memoryCache)
        {
            _unitOfWork = uow;
        }


        public async Task<IEnumerable<ContactForm>> GetOrderedByDate()
        {
            logger.LogInformation("EmailService > GetOrderedByDate()");
            var items = await _unitOfWork.Emails.GetOrderedByDate();
            logger.LogInformation("E-mails loaded: {0}", items.Count());
            return items.MapToViewModel();
        }

        public async Task<ReturnValue> Save(ContactForm contactForm)
        {
            logger.LogInformation("CmsService > Save(contactForm: {0})", contactForm.ToString());

            var returnValue = new ReturnValue($"E-mail saved.");

            try
            {
                logger.LogInformation("New e-mail");
                var message = contactForm.MapToModel();
                message.DateReceived = DateTime.Now;

                await _unitOfWork.Emails.Insert(message);

                await _unitOfWork.Save();
                logger.LogInformation("E-mail saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the e-mail");
            }

            return returnValue;
        }
    }
}
