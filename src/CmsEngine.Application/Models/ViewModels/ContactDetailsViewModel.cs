namespace CmsEngine.Application.Models.ViewModels;

public class ContactDetailsViewModel
{
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public bool HasEmail {
        get {
            return !string.IsNullOrWhiteSpace(Email);
        }
    }
}
