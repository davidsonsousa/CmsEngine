namespace CmsEngine.Application.Models.ViewModels.ManageViewModels;

public class IndexViewModel
{
    public string Username { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public bool IsEmailConfirmed { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    public string StatusMessage { get; set; } = string.Empty;
}
