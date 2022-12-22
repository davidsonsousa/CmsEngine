namespace CmsEngine.Application.Extensions.Mapper;

public static class PostExtensions
{
    /// <summary>
    /// Maps Post model into a PostEditModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static PostEditModel MapToEditModel(this Post item)
    {
        return new PostEditModel
        {
            Id = item.Id,
            VanityId = item.VanityId,
            Title = item.Title,
            Slug = item.Slug,
            Description = item.Description,
            DocumentContent = item.DocumentContent,
            HeaderImage = item.HeaderImage,
            PublishedOn = item.PublishedOn,
            Status = item.Status,
            SelectedCategories = item.PostCategories.Select(x => x.Category.VanityId.ToString()),
            SelectedTags = item.PostTags.Select(x => x.Tag.VanityId.ToString())
        };
    }

    /// <summary>
    /// Maps an IEnumerable<Post> into an IEnumerable<PostEditModel>
    /// </summary>
    /// <param name="posts"></param>
    /// <returns></returns>
    public static IEnumerable<PostEditModel> MapToEditModel(this IEnumerable<Post> posts)
    {
        var editModels = new List<PostEditModel>();

        foreach (var item in posts)
        {
            editModels.Add(new PostEditModel
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
    /// Maps a PostEditModel into a Post
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Post MapToModel(this PostEditModel item)
    {
        return new Post
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
    /// Maps a PostEditModel into a specific Post
    /// </summary>
    /// <param name="item"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    public static Post MapToModel(this PostEditModel item, Post post)
    {
        post.Id = item.Id;
        post.VanityId = item.VanityId;
        post.Title = item.Title;
        post.Slug = item.Slug;
        post.Description = item.Description;
        post.DocumentContent = item.DocumentContent;
        post.HeaderImage = item.HeaderImage;
        post.PublishedOn = item.PublishedOn;
        post.Status = item.Status;

        return post;
    }

    /// <summary>
    /// Maps an IEnumerable<Post> into an IEnumerable<PostTableViewModel>
    /// </summary>
    /// <param name="posts"></param>
    /// <returns></returns>
    public static IEnumerable<PostTableViewModel> MapToTableViewModel(this IEnumerable<Post> posts)
    {
        var tableViewModel = new List<PostTableViewModel>();

        foreach (var item in posts)
        {
            tableViewModel.Add(new PostTableViewModel
            {
                VanityId = item.VanityId,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                PublishedOn = item.PublishedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                Status = item.Status,
                Author = item.ApplicationUsers.MapToViewModelSimple().Single()
            });
        }

        return tableViewModel;
    }

    /// <summary>
    /// Maps Post model into a PostViewModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static PostViewModel MapToViewModel(this Post item, string dateFormat)
    {
        return new PostViewModel
        {
            VanityId = item.VanityId,
            Title = item.Title,
            Slug = item.Slug,
            Description = item.Description,
            DocumentContent = item.DocumentContent,
            HeaderImage = item.HeaderImage,
            PublishedOn = item.PublishedOn.ToString(dateFormat),
            Categories = item.Categories.MapToViewModelSimple(),
            Author = item.ApplicationUsers.MapToViewModelSimple().Single()
        };
    }

    /// <summary>
    /// Maps Post model into a PostViewModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<PostViewModel> MapToViewModel(this IEnumerable<Post> posts, string dateFormat)
    {
        var viewModels = new List<PostViewModel>();

        foreach (var item in posts)
        {
            viewModels.Add(new PostViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn.ToString(dateFormat),
                Status = item.Status
            });
        }

        return viewModels;
    }

    /// <summary>
    /// Maps Post model into a PostViewModel with Categories
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<PostViewModel> MapToViewModelWithCategories(this IEnumerable<Post> posts, string dateFormat)
    {
        var viewModels = new List<PostViewModel>();

        foreach (var item in posts)
        {
            viewModels.Add(new PostViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn.ToString(dateFormat),
                Status = item.Status,
                Categories = item.PostCategories.Select(x => x.Category).MapToViewModel(dateFormat)
            });
        }

        return viewModels;
    }

    /// <summary>
    /// Maps Post model into a PostViewModel with Tags
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<PostViewModel> MapToViewModelWithTags(this IEnumerable<Post> posts, string dateFormat)
    {
        var viewModels = new List<PostViewModel>();

        foreach (var item in posts)
        {
            viewModels.Add(new PostViewModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn.ToString(dateFormat),
                Status = item.Status,
                Tags = item.PostTags.Select(x => x.Tag).MapToViewModel()
            });
        }

        return viewModels;
    }

    /// <summary>
    /// Maps limited information for Partial Views
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<PostViewModel> MapToViewModelForPartialView(this IEnumerable<Post> posts, string dateFormat)
    {
        var viewModels = new List<PostViewModel>();

        foreach (var item in posts)
        {
            viewModels.Add(new PostViewModel
            {
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn.ToString(dateFormat),
                Categories = item.Categories.MapToViewModelSimple(),
                Author = item.ApplicationUsers.MapToViewModelSimple().Single()
            });
        }

        return viewModels;
    }

    /// <summary>
    /// Maps limited information for Partial Views for Tags
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<PostViewModel> MapToViewModelForPartialViewForTags(this IEnumerable<Post> posts, string dateFormat)
    {
        var viewModels = new List<PostViewModel>();

        foreach (var item in posts)
        {
            viewModels.Add(new PostViewModel
            {
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn.ToString(dateFormat),
                Categories = item.Categories.MapToViewModelSimple(),
                Tags = item.Tags.MapToViewModelSimple(),
                Author = item.ApplicationUsers.MapToViewModelSimple().Single()
            });
        }

        return viewModels;
    }
}
