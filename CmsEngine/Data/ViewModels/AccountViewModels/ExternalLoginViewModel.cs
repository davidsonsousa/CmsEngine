using System.ComponentModel.DataAnnotations;

namespace CmsEngine.Data.ViewModels.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
