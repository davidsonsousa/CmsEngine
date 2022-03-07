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

        var message = PrepareMailMessage(contactForm);

        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_emailSettings.Domain, _emailSettings.Port, true);
                await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }

            _logger.LogDebug("Email sent from {0} to {1}", message.From[0], message.To[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when sending e-mail");
            throw new EmailException("Error when sending e-mail", ex);
        }
    }

    private MimeMessage PrepareMailMessage(ContactForm contactForm)
    {
        var from = contactForm.From;
        var body = contactForm.Message;

        if (string.IsNullOrWhiteSpace(from))
        {
            from = _emailSettings.Username;
            body = $"From: {from}\r\nTo: {contactForm.To}\r\n-----\r\n\r\n{body}";
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, from));
        message.Subject = $"🌐 CmsEngine - {contactForm.Subject}";

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        if (!string.IsNullOrWhiteSpace(contactForm.To))
        {
            message.To.Add(new MailboxAddress(contactForm.To, contactForm.To));
        }

        return message;
    }
}
