namespace CmsEngine.Application.Extensions.Mapper;

public static class TagExtensions
{
    extension(Tag item)
    {
        /// <summary>
        /// Maps Tag model into a TagEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TagEditModel MapToEditModel()
        {
            return new TagEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug
            };
        }
    }

    extension(TagEditModel item)
    {
        /// <summary>
        /// Maps a TagEditModel into a Tag
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Tag MapToModel()
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
        public Tag MapToModel(Tag tag)
        {
            tag.Id = item.Id;
            tag.VanityId = item.VanityId;
            tag.Name = item.Name;
            tag.Slug = item.Slug;

            return tag;
        }
    }

    extension(IEnumerable<Tag> tags)
    {
        /// <summary>
        /// Maps an IEnumerable<Tag> into an IEnumerable<TagTableViewModel>
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public IEnumerable<TagTableViewModel> MapToTableViewModel()
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

        /// <summary>
        /// Maps an IEnumerable<Tag> into an IEnumerable<TagViewModel>
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public IEnumerable<TagViewModel> MapToViewModel()
        {
            return tags.Select(item => new TagViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug
            }).ToList();
        }

        /// <summary>
        /// Maps VanityId, Name and Slug
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public IEnumerable<TagViewModel> MapToViewModelSimple()
        {
            return tags.Select(item => new TagViewModel
            {
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug
            }).ToList();
        }
    }
}