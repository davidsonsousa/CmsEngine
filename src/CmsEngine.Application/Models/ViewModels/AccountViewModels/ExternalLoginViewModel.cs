namespace CmsEngine.Application.Models.ViewModels.AccountViewModels;

public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
