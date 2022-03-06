namespace CmsEngine.Data;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IPageRepository Pages { get; }
    IPostRepository Posts { get; }
    ITagRepository Tags { get; }
    IWebsiteRepository Websites { get; }
    UserManager<ApplicationUser> Users { get; }
    IEmailRepository Emails { get; }

    /// <summary>
    /// Saves all pending changes into the database
    /// </summary>
    Task Save();
}
