namespace CmsEngine.Application.Models.EditModels;

public class UserEditModel : BaseEditModel, IEditModel
{
    public string FullName {
        get {
            return $"{Name} {Surname}";
        }
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string UserName { get; set; }
}
