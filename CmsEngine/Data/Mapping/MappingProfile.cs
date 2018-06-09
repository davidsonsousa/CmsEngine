using System.Linq;
using AutoMapper;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;

namespace CmsEngine.Data.Mapper
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
            MapUsers();
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
            CreateMap<Post, PostTableViewModel>();
            CreateMap<PostTableViewModel, Post>();

            // Table view model
            CreateMap<Post, PostTableViewModel>();
            CreateMap<PostTableViewModel, Post>();
        }

        private void MapPage()
        {
            // Edit model
            CreateMap<Page, PageEditModel>();
            CreateMap<PageEditModel, Page>()
                .ForMember(em => em.Id, opt => opt.Ignore())
                .ForMember(em => em.VanityId, opt => opt.Ignore());

            // View model
            CreateMap<Page, PageTableViewModel>();
            CreateMap<PageTableViewModel, Page>();

            // Table view model
            CreateMap<Page, PageTableViewModel>();
            CreateMap<PageTableViewModel, Page>();
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

            // Table view model
            CreateMap<Category, CategoryTableViewModel>();
            CreateMap<CategoryTableViewModel, Category>();
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

            // Table view model
            CreateMap<Tag, TagTableViewModel>();
            CreateMap<TagTableViewModel, Tag>();
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

        private void MapUsers()
        {
            // Edit model
            CreateMap<ApplicationUser, UserEditModel>()
                .ForMember(dest => dest.VanityId, opt => opt.ResolveUsing<UsersStringToGuidResolver>())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserEditModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing<UsersGuidToStringResolver>());

            // View model
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.VanityId, opt => opt.ResolveUsing<UsersStringToGuidResolver>())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing<UsersGuidToStringResolver>());
        }
    }
}
