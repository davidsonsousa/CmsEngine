namespace CmsEngine.Application.Extensions.Mapper;

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
        return categories.Select(item => new CategoryTableViewModel
        {
            Id = item.Id,
            VanityId = item.VanityId,
            Name = item.Name,
            Description = item.Description,
            Slug = item.Slug
        }).ToList();
    }

    /// <summary>
    /// Maps an IEnumerable<Category> into an IEnumerable<CategoryViewModel>
    /// </summary>
    /// <param name="categories"></param>
    /// <returns></returns>
    public static IEnumerable<CategoryViewModel> MapToViewModel(this IEnumerable<Category> categories, string dateFormat)
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
    public static IEnumerable<CategoryViewModel> MapToViewModelSimple(this IEnumerable<Category> categories)
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
    public static IEnumerable<CategoryViewModel> MapToViewModelWithPostCount(this IEnumerable<Category> categories)
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
    public static IEnumerable<CategoryViewModel> MapToViewModelWithPost(this IEnumerable<Category> categories, string dateFormat)
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
}
