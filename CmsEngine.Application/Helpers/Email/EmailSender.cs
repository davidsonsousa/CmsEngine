using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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

        public async Task SendEmailAsync(ContactForm mailEditModel)
        {
            await Execute(mailEditModel);
        }

        private async Task Execute(ContactForm mailEditModel)
        {
            _logger.LogInformation(mailEditModel.ToString());

            try
            {
                string from = mailEditModel.From ?? _emailSettings.Username;

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = $"🌐 CmsEngine - {mailEditModel.Subject}",
                    Body = mailEditModel.Message,
                    IsBodyHtml = false,
                    Priority = MailPriority.Normal
                };

                if (!string.IsNullOrWhiteSpace(mailEditModel.To))
                {
                    mail.To.Add(mailEditModel.To);
                }

                if (!string.IsNullOrWhiteSpace(_emailSettings.CcEmail))
                {
                    mail.CC.Add(_emailSettings.CcEmail);
                }

                if (!string.IsNullOrWhiteSpace(_emailSettings.BccEmail))
                {
                    mail.Bcc.Add(_emailSettings.BccEmail);
                }

                using (var smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
                {
                    smtp.EnableSsl = true;
                    smtp.Timeout = 10000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when sending e-mail");
                throw;
            }
        }
    }
}
