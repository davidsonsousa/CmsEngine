namespace CmsEngine.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CmsEngineContext _ctx;
    private bool _disposed;

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

        _disposed = false;
    }

    public async Task Save()
    {
        try
        {
            await _ctx.SaveChangesAsync();
        }
        // TODO: Implement EF Core error handling
        //catch (DbEntityValidationException ex)
        //{
        //    // Retrieve the error messages as a list of strings.
        //    var errorMessages = ex.EntityValidationErrors
        //                            .SelectMany(x => x.ValidationErrors)
        //                            .Select(x => x.ErrorMessage);

        //    // Join the list to a single string.
        //    var fullErrorMessage = string.Join("; ", errorMessages);

        //    // Combine the original exception message with the new one.
        //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //    // Throw a new DbEntityValidationException with the improved exception message.
        //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //}
        catch (DbUpdateConcurrencyException ex)
        {
            ex.Entries[0].Reload();
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            throw new DbUpdateException(innerException.Message, innerException);
        }
        catch
        {
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _ctx.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
