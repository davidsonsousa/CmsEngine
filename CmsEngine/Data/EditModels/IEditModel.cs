using System;

namespace CmsEngine.Data.EditModels
{
    public interface IEditModel
    {
        int Id { get; set; }
        Guid VanityId { get; set; }
    }
}