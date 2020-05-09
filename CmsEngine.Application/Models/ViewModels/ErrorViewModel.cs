namespace CmsEngine.Application.ViewModels
{
    public class ErrorViewModel
    {
        public string Title { get; }
        public string Message { get; }

        public ErrorViewModel(string message)
        {
            Title = "Error";
            Message = message;
        }

        public ErrorViewModel(string pageTitle, string message)
        {
            Title = pageTitle;
            Message = message;
        }
    }
}
