namespace CmsEngine.Application.Extensions.Mapper;

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
        return pages.Select(item => new PageEditModel
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
        }).ToList();
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
        return pages.Select(item => new PageTableViewModel
        {
            VanityId = item.VanityId,
            Title = item.Title,
            Description = item.Description,
            Slug = item.Slug,
            PublishedOn = item.PublishedOn.ToString("yyyy-MM-dd HH:mm:ss"),
            Status = item.Status,
            Author = item.ApplicationUsers.MapToViewModelSimple().Single()
        }).ToList();
    }

    /// <summary>
    /// Maps Page model into a PageViewModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static PageViewModel MapToViewModel(this Page item, string dateFormat)
    {
        return new PageViewModel
        {
            VanityId = item.VanityId,
            Title = item.Title,
            Slug = item.Slug,
            Description = item.Description,
            DocumentContent = item.DocumentContent,
            HeaderImage = item.HeaderImage,
            PublishedOn = item.PublishedOn.ToString(dateFormat),
            Author = item.ApplicationUsers.MapToViewModelSimple().Single()
        };
    }

    /// <summary>
    /// Maps an IEnumerable<Page> into an IEnumerable<PageEditModel>
    /// </summary>
    /// <param name="pages"></param>
    /// <returns></returns>
    public static IEnumerable<PageViewModel> MapToViewModel(this IEnumerable<Page> pages, string dateFormat)
    {
        return pages.Select(item => new PageViewModel
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
        }).ToList();
    }

}
