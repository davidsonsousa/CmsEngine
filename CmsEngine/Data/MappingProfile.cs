using AutoMapper;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;

namespace CmsEngine.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapWebsite();
        }

        private void MapWebsite()
        {
            CreateMap<Website, WebsiteEditModel>();
            CreateMap<Website, WebsiteViewModel>();

            CreateMap<WebsiteEditModel, Website>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());
            CreateMap<WebsiteViewModel, Website>();
        }
    }
}
