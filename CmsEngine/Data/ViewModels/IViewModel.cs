using System;

namespace CmsEngine.Data.ViewModels
{
    public interface IViewModel
    {
        int Id { get; set; }

        Guid VanityId { get; set; }
    }
}
