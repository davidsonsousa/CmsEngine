using System;

namespace CmsEngine.Application.ViewModels
{
    public interface IViewModel
    {
        int Id { get; set; }

        Guid VanityId { get; set; }
    }
}
