using System;

namespace CmsEngine.Data.ViewModels
{
    public class BaseViewModel : IViewModel
    {
        public int Id { get; set; }
        public Guid VanityId { get; set; }
    }
}
