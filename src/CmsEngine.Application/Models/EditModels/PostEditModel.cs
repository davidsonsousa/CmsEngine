namespace CmsEngine.Application.Models.EditModels;

public class PostEditModel : BaseEditModel, IEditModel
{
    [Required]
    [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
    public string Title { get; set; }

    public string Slug { get; set; }

    public string? HeaderImage { get; set; }

    [Required]
    [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
    public string Description { get; set; }

    public string DocumentContent { get; set; }

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
        var jsonResult = new JObject(
                                    new JProperty("Id", Id),
                                    new JProperty("VanityId", VanityId),
                                    new JProperty("Title", Title),
                                    new JProperty("Slug", Slug),
                                    new JProperty("HeaderImage", HeaderImage),
                                    new JProperty("Description", Description),
                                    new JProperty("Status", Status.ToString()),
                                    new JProperty("PublishedOn", PublishedOn)
                                );
        return jsonResult.ToString();
    }
}
