namespace CmsEngine.Application.Models.EditModels;

public class UserEditModel : BaseEditModel, IEditModel
{
    public string FullName {
        get {
            return $"{Name} {Surname}";
        }
    }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
}
