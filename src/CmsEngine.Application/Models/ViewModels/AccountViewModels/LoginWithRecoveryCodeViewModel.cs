namespace CmsEngine.Application.Models.ViewModels.AccountViewModels;

public class LoginWithRecoveryCodeViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Recovery Code")]
    public string RecoveryCode { get; set; }
}
