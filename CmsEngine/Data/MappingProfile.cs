using System.Linq;
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
            MapPost();
            MapPage();
            MapCategory();
            MapTag();
            MapWebsite();
        }

        private void MapPost()
        {
            // Edit model
            CreateMap<Post, PostEditModel>()
                .ForMember(
                    em => em.SelectedCategories,
                    opt => opt.MapFrom(p => p.PostCategories.Select(x => x.Category.VanityId.ToString()).ToList())
                );

            CreateMap<PostEditModel, Post>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Post, PostViewModel>();
            CreateMap<PostViewModel, Post>();
        }

        private void MapPage()
        {
            // Edit model
            CreateMap<Page, PageEditModel>();
            CreateMap<PageEditModel, Page>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Page, PageViewModel>();
            CreateMap<PageViewModel, Page>();
        }

        private void MapCategory()
        {
            // Edit model
            CreateMap<Category, CategoryEditModel>();
            CreateMap<CategoryEditModel, Category>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
        }

        private void MapTag()
        {
            // Edit model
            CreateMap<Tag, TagEditModel>();
            CreateMap<TagEditModel, Tag>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Tag, TagViewModel>();
            CreateMap<TagViewModel, Tag>();
        }

        private void MapWebsite()
        {
            // Edit model
            CreateMap<Website, WebsiteEditModel>();
            CreateMap<WebsiteEditModel, Website>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Website, WebsiteViewModel>();
            CreateMap<WebsiteViewModel, Website>();
        }
    }
}
