using System;

namespace CmsEngine.Domain.ViewModels
{
    public class BaseViewModel : IViewModel
    {
        public int Id { get; set; }
        public Guid VanityId { get; set; }
    }
}
