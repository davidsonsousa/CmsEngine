namespace CmsEngine.Application.Models.EditModels;

public class PostEditModel : BaseEditModel, IEditModel
{
    [Required]
    [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? HeaderImage { get; set; }

    [Required]
    [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
    public string Description { get; set; } = string.Empty;

    public string DocumentContent { get; set; } = string.Empty;

    public IEnumerable<CheckboxEditModel> Categories { get; set; } = new List<CheckboxEditModel>();

    public IEnumerable<string> SelectedCategories { get; set; } = new List<string>();

    // TODO: Perhaps replace the SelectListItem by something else in order to make it less ASP.NET Core dependent
    public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();

    public IEnumerable<string> SelectedTags { get; set; } = new List<string>();

    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

    [Required]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime PublishedOn { get; set; } = DateTime.Now;

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId,
            [nameof(Title)] = Title,
            [nameof(Slug)] = Slug,
            [nameof(HeaderImage)] = HeaderImage,
            [nameof(Description)] = Description,
            [nameof(Status)] = Status.ToString(),
            [nameof(PublishedOn)] = PublishedOn
        };
        return jsonResult.ToString();
    }
}
