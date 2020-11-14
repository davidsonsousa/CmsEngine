using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CmsEngine.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CmsEngine.Application.Helpers.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
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

            string from = contactForm.From ?? _emailSettings.Username;
            string body = $"From: {from}\r\nTo: {contactForm.To}\r\n-----\r\n\r\n{contactForm.Message}";

            try
            {
                MailMessage message = new MailMessage
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
}
