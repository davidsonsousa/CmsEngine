namespace CmsEngine.Domain.ViewModels
{
    public class PageViewModel : DocumentViewModel
    {
        public UserViewModel Author { get; set; } = new UserViewModel();
    }
}
