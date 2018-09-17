using System.Collections.Generic;

namespace CmsEngine.Data.ViewModels
{
    public class PostViewModel : DocumentViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
        public UserViewModel Author { get; set; } = new UserViewModel();
    }
}


