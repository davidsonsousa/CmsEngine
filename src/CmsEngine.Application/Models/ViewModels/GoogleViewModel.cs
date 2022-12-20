namespace CmsEngine.Application.Models.ViewModels;

public class GoogleViewModel
{
    public string? GoogleAnalytics { get; set; }

    public string? GoogleRecaptchaSiteKey { get; set; }

    public string? GoogleRecaptchaSecretKey { get; set; }

    public bool HasAnalytics {
        get {
            return !string.IsNullOrWhiteSpace(GoogleAnalytics);
        }
    }

    public bool HasRecaptchaSiteKey {
        get {
            return !string.IsNullOrWhiteSpace(GoogleRecaptchaSiteKey);
        }
    }

    public bool HasRecaptchaSecretKey {
        get {
            return !string.IsNullOrWhiteSpace(GoogleRecaptchaSecretKey);
        }
    }
}
