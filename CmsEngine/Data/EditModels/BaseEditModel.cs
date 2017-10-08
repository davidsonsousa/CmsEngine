using System;

namespace CmsEngine.Data.EditModels
{
    public class BaseEditModel
    {
        public bool IsNew
        {
            get
            {
                return (Id == 0 && VanityId == Guid.Empty);
            }
        }

        public int Id { get; set; }

        public Guid VanityId { get; set; }
    }
}
