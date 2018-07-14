using AutoMapper;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;

namespace CmsEngine.Data.Mapper
{
    internal class UsersGuidToStringResolver : IValueResolver<object, ApplicationUser, string>
    {
        public string Resolve(object source, ApplicationUser destination, string destMember, ResolutionContext context)
        {
            switch (source)
            {
                case IEditModel editModel:
                    return editModel.VanityId.ToString();
                case IViewModel viewModel:
                    return viewModel.VanityId.ToString();
                case null:
                default:
                    return string.Empty;
            }
        }
    }
}
