namespace CmsEngine.Application.Models.EditModels;

public class TagEditModel : BaseEditModel, IEditModel
{
    [Required]
    [MaxLength(15, ErrorMessage = "The name must have less than 15 characters")]
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;
}
