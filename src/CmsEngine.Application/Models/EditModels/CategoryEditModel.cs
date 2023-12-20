namespace CmsEngine.Application.Models.EditModels;

public class CategoryEditModel : BaseEditModel, IEditModel
{
    [Required]
    [MaxLength(35, ErrorMessage = "The name must have less than 35 characters")]
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId,
            [nameof(Name)] = Name,
            [nameof(Slug)] = Slug,
            [nameof(Description)] = Description
        };
        return jsonResult.ToString();
    }
}
