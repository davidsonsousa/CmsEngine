using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CmsEngine.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        #region Navigation

        public virtual ICollection<PostApplicationUser> PostApplicationUsers { get; set; }
        public virtual ICollection<PageApplicationUser> PageApplicationUsers { get; set; }

        #endregion
    }
}
