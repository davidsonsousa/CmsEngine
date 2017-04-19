using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
