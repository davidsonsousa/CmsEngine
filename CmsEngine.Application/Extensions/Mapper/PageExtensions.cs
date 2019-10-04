using System.Collections.Generic;
using System.Linq;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Data.Entities;

namespace CmsEngine.Application.Extensions.Mapper
{
    public static class PageExtensions
    {
        /// <summary>
        /// Maps Page model into a PageEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static PageEditModel MapToEditModel(this Page item)
        {
            return new PageEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn,
                Status = item.Status
            };
        }

        /// <summary>
        /// Maps an IEnumerable<Page> into an IEnumerable<PageEditModel>
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static IEnumerable<PageEditModel> MapToEditModel(this IEnumerable<Page> pages)
        {
            var editModels = new List<PageEditModel>();

            foreach (var item in pages)
            {
                editModels.Add(new PageEditModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    DocumentContent = item.DocumentContent,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = item.PublishedOn,
                    Status = item.Status
                });
            }

            return editModels;
        }

        /// <summary>
        /// Maps a PageEditModel into a Page
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Page MapToModel(this PageEditModel item)
        {
            return new Page
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn,
                Status = item.Status
            };
        }

        /// <summary>
        /// Maps a PageEditModel into a specific Page
        /// </summary>
        /// <param name="item"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Page MapToModel(this PageEditModel item, Page page)
        {
            page.Id = item.Id;
            page.VanityId = item.VanityId;
            page.Title = item.Title;
            page.Slug = item.Slug;
            page.Description = item.Description;
            page.DocumentContent = item.DocumentContent;
            page.HeaderImage = item.HeaderImage;
            page.PublishedOn = item.PublishedOn;
            page.Status = item.Status;

            return page;
        }

        /// <summary>
        /// Maps an IEnumerable<Page> into an IEnumerable<PageTableViewModel>
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static IEnumerable<PageTableViewModel> MapToTableViewModel(this IEnumerable<Page> pages)
        {
            var tableViewModel = new List<PageTableViewModel>();

            foreach (var item in pages)
            {
                tableViewModel.Add(new PageTableViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    DocumentContent = item.DocumentContent,
                    Author = item.PageApplicationUsers.Select(x => x.ApplicationUser).SingleOrDefault().MapToViewModel(),
                    PublishedOn = item.PublishedOn,
                    Status = item.Status
                });
            }

            return tableViewModel;
        }
    }
}
