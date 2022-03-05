namespace CmsEngine.Application.Models.ViewModels;

public class SocialMediaViewModel
{
    public string Facebook { get; set; }
    public string Twitter { get; set; }
    public string Instagram { get; set; }
    public string LinkedIn { get; set; }

    public bool HasFacebook {
        get {
            return !string.IsNullOrWhiteSpace(Facebook);
        }
    }

    public bool HasTwitter {
        get {
            return !string.IsNullOrWhiteSpace(Twitter);
        }
    }

    public bool HasInstagram {
        get {
            return !string.IsNullOrWhiteSpace(Instagram);
        }
    }

    public bool HasLinkedIn {
        get {
            return !string.IsNullOrWhiteSpace(LinkedIn);
        }
    }

    public bool HasSocialMedia {
        get {
            return HasFacebook || HasTwitter || HasInstagram || HasLinkedIn;
        }
    }
}
