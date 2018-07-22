using System;
using AutoMapper;
using CmsEngine.Data.Models;

namespace CmsEngine.Data.Mapper
{
    internal class UsersStringToGuidResolver : IValueResolver<ApplicationUser, object, Guid>
    {
        public Guid Resolve(ApplicationUser source, object destination, Guid destMember, ResolutionContext context)
        {
            if (Guid.TryParse(source.Id, out Guid returnValue))
            {
                return returnValue;
            }

            return Guid.Empty;
        }
    }
}
