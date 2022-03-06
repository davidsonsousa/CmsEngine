namespace CmsEngine.Application.Services;

public class Service : IService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMemoryCache memoryCache;
    private readonly string instanceHost;
    private readonly string instanceKey;

    protected readonly IUnitOfWork unitOfWork;
    protected readonly ILogger logger;

    public InstanceViewModel Instance {
        get {
            return GetInstance();
        }
    }
    public UserViewModel CurrentUser {
        get {
            return GetCurrentUserViewModelAsync().GetAwaiter().GetResult();
        }
    }

    public Service(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
    {
        unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
        httpContextAccessor = hca;
        logger = loggerFactory.CreateLogger("Service");
        this.memoryCache = memoryCache;

        instanceHost = httpContextAccessor.HttpContext.Request.Host.Host;
        instanceKey = $"{Main.CacheKey.Instance}_{instanceHost}";
    }

    internal async Task<ApplicationUser> GetCurrentUserAsync()
    {
        logger.LogDebug("GetCurrentUserAsync() for {0}", httpContextAccessor.HttpContext.User.Identity.Name);

        try
        {
            return await unitOfWork.Users.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when trying to load CurrentUser");
            throw;
        }
    }

    protected void SaveInstanceToCache(object instance)
    {
        var timeSpan = TimeSpan.FromDays(7); //TODO: Perhaps set this in the config file. Or DB
        logger.LogDebug("Adding '{0}' to cache with expiration date to {1}", instanceKey, DateTime.Now.AddMilliseconds(timeSpan.TotalMilliseconds).ToString());
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(timeSpan);
        memoryCache.Set(instanceKey, instance, cacheEntryOptions);
    }

    private InstanceViewModel GetInstance()
    {
        logger.LogDebug("GetInstanceAsync()");

        Website website;
        InstanceViewModel instance;

        try
        {
            logger.LogDebug("Loading '{0}' from cache", instanceKey);
            if (!memoryCache.TryGetValue(instanceKey, out instance))
            {
                logger.LogDebug("Empty cache for '{0}'. Loading instance from DB", instanceKey);
                website = unitOfWork.Websites.GetWebsiteInstanceByHost(instanceHost);

                if (website == null)
                {
                    throw new NotFoundException($"Instance for '{instanceHost}' not found");
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
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when trying to load Instance");
            throw;
        }

        return instance;
    }

    private async Task<UserViewModel> GetCurrentUserViewModelAsync()
    {
        var user = await GetCurrentUserAsync();

        return user == null
            ? null
            : new UserViewModel
            {
                VanityId = Guid.Parse(user.Id),
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName
            };
    }
}
