namespace CmsEngine.Application.Models.ViewModels;

public class PageViewModel : DocumentViewModel
{
    public UserViewModel Author { get; set; } = new UserViewModel();
}
