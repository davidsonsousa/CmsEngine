namespace CmsEngine.Application.Helpers.Email;

public class CmsEngineEmailSender : ICmsEngineEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<CmsEngineEmailSender> _logger;

    public CmsEngineEmailSender(IOptions<EmailSettings> emailSettings, ILogger<CmsEngineEmailSender> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(ContactForm contactForm)
    {
        await ExecuteAsync(contactForm);
    }

    private async Task ExecuteAsync(ContactForm contactForm)
    {
        _logger.LogDebug("SendEmailAsync(contactForm: {0})", contactForm.ToString());

        var from = contactForm.From ?? _emailSettings.Username;
        var body = $"From: {from}\r\nTo: {contactForm.To}\r\n-----\r\n\r\n{contactForm.Message}";

        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = $"🌐 CmsEngine - {contactForm.Subject}",
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = false,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                Priority = MailPriority.Normal
            };

            if (!string.IsNullOrWhiteSpace(contactForm.To))
            {
                message.To.Add(contactForm.To);
            }

            if (!string.IsNullOrWhiteSpace(_emailSettings.CcEmail))
            {
                message.CC.Add(_emailSettings.CcEmail);
            }

            if (!string.IsNullOrWhiteSpace(_emailSettings.BccEmail))
            {
                message.Bcc.Add(_emailSettings.BccEmail);
            }

            using (var smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
            {
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                _logger.LogDebug("Message {0}", message.ToString());
                await smtp.SendMailAsync(message);
            }

            _logger.LogDebug("Email sent from {0} to {1}", message.From, message.To[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when sending e-mail");
            throw new EmailException("Error when sending e-mail", ex);
        }
    }
}
