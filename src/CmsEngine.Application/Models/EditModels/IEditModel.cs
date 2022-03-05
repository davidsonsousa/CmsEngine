namespace CmsEngine.Application.Models.EditModels;

public interface IEditModel
{
    bool IsNew { get; }

    int Id { get; set; }
    Guid VanityId { get; set; }
}
