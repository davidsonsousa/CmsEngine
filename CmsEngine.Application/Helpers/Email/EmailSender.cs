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

                MailMessage mail = new MailMessage(new MailAddress(from), new MailAddress(mailEditModel.To));

                if (!string.IsNullOrWhiteSpace(_emailSettings.CcEmail))
                {
                    mail.CC.Add(new MailAddress(_emailSettings.CcEmail));
                }

                if (!string.IsNullOrWhiteSpace(_emailSettings.BccEmail))
                {
                    mail.Bcc.Add(new MailAddress(_emailSettings.BccEmail));
                }

                mail.Subject = $"🌐 CmsEngine - {mailEditModel.Subject}";
                mail.Body = mailEditModel.Message;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;

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
