namespace CmsEngine.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CmsEngineContext _ctx;
    private readonly ILogger<UnitOfWork> _logger;
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
                      UserManager<ApplicationUser> userManager, IEmailRepository emailRepository, ILogger<UnitOfWork> logger)
    {
        _ctx = context;
        _logger = logger;

        Categories = categoryRepository;
        Pages = pageRepository;
        Posts = postRepository;
        Tags = tagRepository;
        Websites = websiteRepository;
        Users = userManager;
        Emails = emailRepository;

        disposedValue = false;
    }

    public async Task<int> Save(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                await entry.ReloadAsync();
            }

            throw new DbUpdateConcurrencyException("A concurrency conflict occurred. All entries were reloaded.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Unable to update data.");

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to save data.");
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        disposedValue = true;

        if (disposing)
        {
            // Dispose managed resources
            try { Categories?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Categories."); }
            try { Pages?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Pages."); }
            try { Posts?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Posts."); }
            try { Tags?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Tags."); }
            try { Websites?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Websites."); }
            try { Users?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing Users."); }
            try { _ctx?.Dispose(); } catch (Exception ex) { _logger.LogError(ex, "Error disposing DbContext."); }

            // Null out references to help GC
            Categories = null!;
            Pages = null!;
            Posts = null!;
            Tags = null!;
            Websites = null!;
            Users = null!;
            Emails = null!;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
