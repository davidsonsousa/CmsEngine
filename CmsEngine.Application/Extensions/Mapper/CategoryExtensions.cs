using System.Collections.Generic;
using CmsEngine.Data.Entities;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.ViewModels.DataTableViewModels;

namespace CmsEngine.Domain.Extensions.Mapper
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
    }
}
