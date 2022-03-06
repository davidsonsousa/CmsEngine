namespace CmsEngine.Application.Models.ViewModels;

public interface IViewModel
{
    int Id { get; set; }

    Guid VanityId { get; set; }
}
