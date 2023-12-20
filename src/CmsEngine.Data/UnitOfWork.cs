namespace CmsEngine.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CmsEngineContext _ctx;
    private bool disposedValue;

    public ICategoryRepository Categories { get; private set; }
    public IPageRepository Pages { get; private set; }
    public IPostRepository Posts { get; private set; }
    public ITagRepository Tags { get; private set; }
    public IWebsiteRepository Websites { get; private set; }
    public UserManager<ApplicationUser> Users { get; private set; }
    public IEmailRepository Emails { get; private set; }

    public UnitOfWork(CmsEngineContext context, ICategoryRepository categoryRepository, IPageRepository pageRepository,
                      IPostRepository postRepository, ITagRepository tagRepository, IWebsiteRepository websiteRepository,
                      UserManager<ApplicationUser> userManager, IEmailRepository emailRepository)
    {
        _ctx = context;

        Categories = categoryRepository;
        Pages = pageRepository;
        Posts = postRepository;
        Tags = tagRepository;
        Websites = websiteRepository;
        Users = userManager;
        Emails = emailRepository;

        disposedValue = false;
    }

    public async Task Save()
    {
        try
        {
            await _ctx.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            ex.Entries[0].Reload();
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is not null)
            {
                throw new DbUpdateException(innerException.Message, innerException);
            }
            else
            {
                throw;
            }
        }
        catch
        {
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Categories.Dispose();
                Pages.Dispose();
                Posts.Dispose();
                Tags.Dispose();
                Users.Dispose();

                _ctx.Dispose();
            }
        }
        disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
