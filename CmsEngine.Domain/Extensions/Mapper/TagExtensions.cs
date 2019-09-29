using System.Collections.Generic;
using CmsEngine.Data.Entities;
using CmsEngine.Domain.EditModels;
using CmsEngine.Domain.ViewModels.DataTableViewModels;

namespace CmsEngine.Domain.Extensions.Mapper
{
    public static class TagExtensions
    {
        /// <summary>
        /// Maps Tag model into a TagEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TagEditModel MapToEditModel(this Tag item)
        {
            return new TagEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug
            };
        }

        /// <summary>
        /// Maps a TagEditModel into a Tag
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Tag MapToModel(this TagEditModel item)
        {
            return new Tag
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug,
            };
        }

        /// <summary>
        /// Maps a TagEditModel into a specific Tag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Tag MapToModel(this TagEditModel item, Tag tag)
        {
            tag.Id = item.Id;
            tag.VanityId = item.VanityId;
            tag.Name = item.Name;
            tag.Slug = item.Slug;

            return tag;
        }

        /// <summary>
        /// Maps an IEnumerable<Tag> into an IEnumerable<TagTableViewModel>
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static IEnumerable<TagTableViewModel> MapToTableViewModel(this IEnumerable<Tag> tags)
        {
            var tableViewModel = new List<TagTableViewModel>();

            foreach (var item in tags)
            {
                tableViewModel.Add(new TagTableViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Name = item.Name,
                    Slug = item.Slug
                });
            }

            return tableViewModel;
        }
    }
}
