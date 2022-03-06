namespace CmsEngine.Ui.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmationAsync(this ICmsEngineEmailSender emailSender, string email, string link)
    {
        var contactForm = new ContactForm(email,
                                          "Confirm your email",
                                          $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");

        return emailSender.SendEmailAsync(contactForm);
    }

    public static Task SendPasswordResetAsync(this ICmsEngineEmailSender emailSender, string email, string link)
    {
        var contactForm = new ContactForm(email,
                                          "Reset Password",
                                          $"Please reset your password by clicking here: <a href='{link}'>link</a>");

        return emailSender.SendEmailAsync(contactForm);
    }
}
