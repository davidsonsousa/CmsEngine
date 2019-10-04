using System;

namespace CmsEngine.Domain.ViewModels
{
    public interface IViewModel
    {
        int Id { get; set; }

        Guid VanityId { get; set; }
    }
}
