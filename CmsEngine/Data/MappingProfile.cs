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
            MapCategory();
            MapTag();
            MapWebsite();
        }

        private void MapCategory()
        {
            CreateMap<Category, CategoryEditModel>();
            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryEditModel, Category>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());
            CreateMap<CategoryViewModel, Category>();
        }

        private void MapTag()
        {
            CreateMap<Tag, TagEditModel>();
            CreateMap<Tag, TagViewModel>();

            CreateMap<TagEditModel, Tag>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());
            CreateMap<TagViewModel, Tag>();
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
