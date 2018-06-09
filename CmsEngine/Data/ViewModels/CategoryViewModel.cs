namespace CmsEngine.Data.ViewModels
{
    public class CategoryViewModel : BaseViewModel, IViewModel
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }
    }
}
