namespace CmsEngine.Application.Extensions.Mapper;

public static class CategoryExtensions
{
    extension(Category item)
    {
        /// <summary>
        /// Maps Category model into a CategoryEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CategoryEditModel MapToEditModel()
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
    }

    extension(CategoryEditModel item)
    {
        /// <summary>
        /// Maps a CategoryEditModel into a Category
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Category MapToModel()
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
        public Category MapToModel(Category category)
        {
            category.Id = item.Id;
            category.VanityId = item.VanityId;
            category.Name = item.Name;
            category.Description = item.Description;
            category.Slug = item.Slug;

            return category;
        }
    }

    extension(IEnumerable<Category> categories)
    {
        /// <summary>
        /// Maps an IEnumerable<Category> into an IEnumerable<CategoryViewModel>
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<CategoryViewModel> MapToViewModel(string dateFormat)
        {
            return categories.Select(item => new CategoryViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug,
                Posts = item.PostCategories?.Select(x => x.Post).MapToViewModel(dateFormat)
            }).ToList();
        }

        /// <summary>
        /// Maps VanityId, Name and Slug
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<CategoryViewModel> MapToViewModelSimple()
        {
            return categories.Select(item => new CategoryViewModel
            {
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug
            }).ToList();
        }

        /// <summary>
        /// Maps VanityId, Name and Slug with post count
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<CategoryViewModel> MapToViewModelWithPostCount()
        {
            return categories.Select(item => new CategoryViewModel
            {
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug,
                PostCount = item.PostCount
            }).ToList();
        }

        /// <summary>
        /// Maps VanityId, Name and Slug with Posts
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<CategoryViewModel> MapToViewModelWithPost(string dateFormat)
        {
            return categories.Select(item => new CategoryViewModel
            {
                VanityId = item.VanityId,
                Name = item.Name,
                Slug = item.Slug,
                Posts = item.Posts.Select(p => new PostViewModel
                {
                    VanityId = p.VanityId,
                    Title = p.Title,
                    Description = p.Description,
                    Slug = p.Slug,
                    PublishedOn = p.PublishedOn.ToString(dateFormat)
                })
            }).ToList();
        }

        /// <summary>
        /// Maps an IEnumerable<Category> into an IEnumerable<CategoryTableViewModel>
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<CategoryTableViewModel> MapToTableViewModel()
        {
            return categories.Select(item => new CategoryTableViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Name = item.Name,
                Description = item.Description,
                Slug = item.Slug
            }).ToList();
        }
    }
}