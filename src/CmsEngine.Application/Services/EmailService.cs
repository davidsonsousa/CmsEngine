namespace CmsEngine.Application.Services;

public class EmailService : Service, IEmailService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmailService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, ICacheService cacheService)
                          : base(uow, hca, loggerFactory, cacheService)
    {
        _unitOfWork = uow;
    }


    public async Task<IEnumerable<ContactForm>> GetOrderedByDate()
    {
        logger.LogDebug("EmailService > GetOrderedByDate()");
        var items = await _unitOfWork.Emails.GetOrderedByDate();
        logger.LogDebug("E-mails loaded: {0}", items.Count());
        return items.MapToViewModel();
    }

    public async Task<ReturnValue> Save(ContactForm contactForm)
    {
        logger.LogDebug("CmsService > Save(contactForm: {0})", contactForm.ToString());

        var returnValue = new ReturnValue($"E-mail saved.");

        try
        {
            logger.LogDebug("New e-mail");
            var message = contactForm.MapToModel();
            message.DateReceived = DateTime.Now;

            await _unitOfWork.Emails.Insert(message);

            await _unitOfWork.Save();
            logger.LogDebug("E-mail saved");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while saving the e-mail");
        }

        return returnValue;
    }
}
