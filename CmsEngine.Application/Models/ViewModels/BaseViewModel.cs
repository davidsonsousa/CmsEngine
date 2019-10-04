using System;

namespace CmsEngine.Application.ViewModels
{
    public class BaseViewModel : IViewModel
    {
        public int Id { get; set; }
        public Guid VanityId { get; set; }
    }
}
