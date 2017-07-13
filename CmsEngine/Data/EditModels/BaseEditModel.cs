using System;
using CmsEngine.Attributes;

namespace CmsEngine.Data.EditModels
{
    public class BaseEditModel
    {
        public bool IsNew => (Id == 0 && VanityId == Guid.Empty);

        public int Id { get; set; }

        public Guid VanityId { get; set; }
    }
}