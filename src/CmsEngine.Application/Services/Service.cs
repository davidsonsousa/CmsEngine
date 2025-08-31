namespace CmsEngine.Application.Services;

public class Service : IService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMemoryCache memoryCache;
    private readonly string instanceHost;
    private readonly string instanceKey;
    private bool disposedValue;

    protected readonly IUnitOfWork unitOfWork;
    protected readonly ILogger logger;

    public InstanceViewModel Instance {
        get {
            return GetInstance();
        }
    }

    public UserViewModel CurrentUser {
        get {
            var currentUser = GetCurrentUserViewModelAsync().GetAwaiter().GetResult();
            return currentUser is null
                   ? throw new Exception("Current user not found")
                   : currentUser;
        }
    }

    public Service(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
    {
        unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
        httpContextAccessor = hca;
        logger = loggerFactory.CreateLogger("Service");
        this.memoryCache = memoryCache;

        instanceHost = httpContextAccessor.HttpContext!.Request.Host.Host;
        instanceKey = $"{Main.CacheKey.Instance}_{instanceHost}";
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }

            disposedValue = true;
        }
    }

    protected int ValidatePage(int page)
    {
        return Math.Max(page, 1);
    }

    protected void SaveInstanceToCache(object instance)
    {
        var timeSpan = TimeSpan.FromHours(1); //TODO: Perhaps set this in the config file. Or DB
        logger.LogDebug("Adding '{instanceKey}' to cache with expiration date to {dateTimeNow}", instanceKey, DateTime.Now.AddMilliseconds(timeSpan.TotalMilliseconds).ToString());
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(timeSpan).SetSize(1);
        memoryCache.Set(instanceKey, instance, cacheEntryOptions);
    }

    protected async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var userName = httpContextAccessor.HttpContext?.User.Identity?.Name ?? "username";
        logger.LogDebug("GetCurrentUserAsync() for {userName}", userName);

        try
        {
            var user = await unitOfWork.Users.FindByNameAsync(userName);
            return user ?? throw new Exception($"User '{userName}' not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when trying to load CurrentUser");
            throw;
        }
    }

    private InstanceViewModel GetInstance()
    {
        logger.LogDebug("GetInstanceAsync()");
        logger.LogDebug("Loading '{instanceKey}' from cache", instanceKey);

        if (memoryCache.TryGetValue(instanceKey, out InstanceViewModel? instance))
        {
            return instance!;
        }

        logger.LogDebug("Empty cache for '{instanceKey}'. Loading instance from DB", instanceKey);
        var website = unitOfWork.Websites.GetWebsiteInstanceByHost(instanceHost);

        if (website == null)
        {
            throw new ItemNotFoundException($"Instance for '{instanceHost}' not found");
        }

        instance = new InstanceViewModel
        {
            Id = website.Id,
            Name = website.Name,
            Description = website.Description,
            Tagline = website.Tagline,
            HeaderImage = website.HeaderImage,
            Culture = website.Culture,
            UrlFormat = website.UrlFormat,
            DateFormat = website.DateFormat,
            SiteUrl = website.SiteUrl,
            ArticleLimit = website.ArticleLimit,
            PageTitle = website.Name,
            ContactDetails = new ContactDetailsViewModel
            {
                Address = website.Address,
                Phone = website.Phone,
                Email = website.Email,
            },
            ApiDetails = new ApiDetailsViewModel
            {
                FacebookAppId = website.FacebookAppId,
                FacebookApiVersion = website.FacebookApiVersion,
                DisqusShortName = website.DisqusShortName
            },
            SocialMedia = new SocialMediaViewModel
            {
                Facebook = website.Facebook,
                Twitter = website.Twitter,
                Instagram = website.Instagram,
                LinkedIn = website.LinkedIn
            },
            Google = new GoogleViewModel
            {
                GoogleAnalytics = website.GoogleAnalytics,
                GoogleRecaptchaSiteKey = website.GoogleRecaptchaSiteKey,
                GoogleRecaptchaSecretKey = website.GoogleRecaptchaSecretKey
            }
        };

        SaveInstanceToCache(instance);

        return instance!;
    }

    private async Task<UserViewModel?> GetCurrentUserViewModelAsync()
    {
        var currentUser = await GetCurrentUserAsync();

        return currentUser is null
            ? throw new Exception("Current user not found")
            : new UserViewModel
            {
                VanityId = Guid.Parse(currentUser.Id),
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                UserName = currentUser.UserName
            };
    }
}
