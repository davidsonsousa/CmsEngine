using System.Collections.Generic;
using System.Linq;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.ViewModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Data.Entities;

namespace CmsEngine.Application.Extensions.Mapper
{
    public static class CategoryExtensions
    {
        /// <summary>
        /// Maps Category model into a CategoryEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static CategoryEditModel MapToEditModel(this Category item)
        {
            return new CategoryEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug
            };
        }

        /// <summary>
        /// Maps a CategoryEditModel into a Category
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Category MapToModel(this CategoryEditModel item)
        {
            return new Category
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug,
            };
        }

        /// <summary>
        /// Maps a CategoryEditModel into a specific Category
        /// </summary>
        /// <param name="item"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static Category MapToModel(this CategoryEditModel item, Category category)
        {
            category.Id = item.Id;
            category.VanityId = item.VanityId;
            category.Name = item.Name;
            category.Description = item.Description;
            category.Slug = item.Slug;

            return category;
        }

        /// <summary>
        /// Maps an IEnumerable<Category> into an IEnumerable<CategoryTableViewModel>
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static IEnumerable<CategoryTableViewModel> MapToTableViewModel(this IEnumerable<Category> categories)
        {
            var tableViewModel = new List<CategoryTableViewModel>();

            foreach (var item in categories)
            {
                tableViewModel.Add(new CategoryTableViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Name = item.Name,
                    Description = item.Description,
                    Slug = item.Slug
                });
            }

            return tableViewModel;
        }

        /// <summary>
        /// Maps an IEnumerable<Category> into an IEnumerable<CategoryViewModel>
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static IEnumerable<CategoryViewModel> MapToViewModel(this IEnumerable<Category> categories)
        {
            var viewModel = new List<CategoryViewModel>();

            foreach (var item in categories)
            {
                viewModel.Add(new CategoryViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Name = item.Name,
                    Description = item.Description,
                    Slug = item.Slug,
                    Posts = item.PostCategories.Select(x => x.Post).MapToViewModel()
                });
            }

            return viewModel;
        }

        /// <summary>
        /// Maps VanityId, Name and Slug
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static IEnumerable<CategoryViewModel> MapToViewModelSimple(this IEnumerable<Category> categories)
        {
            var viewModel = new List<CategoryViewModel>();

            foreach (var item in categories)
            {
                viewModel.Add(new CategoryViewModel
                {
                    VanityId = item.VanityId,
                    Name = item.Name,
                    Slug = item.Slug
                });
            }

            return viewModel;
        }
    }
}
