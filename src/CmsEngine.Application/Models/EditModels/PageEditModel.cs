namespace CmsEngine.Application.Models.EditModels;

public class PageEditModel : BaseEditModel, IEditModel
{
    public PageEditModel()
    {
        Status = DocumentStatus.Draft;
        PublishedOn = DateTime.Now;
    }

    [Required]
    [MaxLength(100, ErrorMessage = "The title must have less than 100 characters")]
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? HeaderImage { get; set; }

    [Required]
    [MaxLength(150, ErrorMessage = "The description must have less than 150 characters")]
    public string Description { get; set; } = string.Empty;

    public string DocumentContent { get; set; } = string.Empty;

    public DocumentStatus Status { get; set; }

    [Required]
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
    public DateTime PublishedOn { get; set; }

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
