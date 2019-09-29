using System.Collections.Generic;

namespace CmsEngine.Domain.ViewModels
{
    public class CategoryViewModel : BaseViewModel, IViewModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
