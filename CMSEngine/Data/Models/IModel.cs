using System;

namespace CmsEngine.Data.Models
{
    public interface IModel
    {
        bool IsNew { get; }
        bool IsDeleted { get; set; }

        int Id { get; set; }
        Guid VanityId { get; set; }

        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }

        string UserCreated { get; set; }
        string UserModified { get; set; }
    }
}
