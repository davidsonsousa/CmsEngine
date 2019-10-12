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

        public async Task SendEmailConfirmationAsync(ContactForm mailEditModel)
        {
            await Execute(mailEditModel);
        }

        public async Task SendPasswordResetAsync(ContactForm mailEditModel)
        {
            await Execute(mailEditModel);
        }

        private async Task Execute(ContactForm mailEditModel)
        {
            try
            {
                MailMessage mail = new MailMessage(new MailAddress(_emailSettings.Username, "CmsEngine"), new MailAddress(mailEditModel.Sender));

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

                using (SmtpClient smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when sending e-mail");
            }
        }
    }
}
