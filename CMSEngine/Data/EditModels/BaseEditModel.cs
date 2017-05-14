using System;

namespace CmsEngine.Data.EditModels
{
    public class BaseEditModel
    {
        public bool IsNew => (Id == 0);

        public int Id { get; set; }

        public Guid VanityId { get; set; }
    }
}