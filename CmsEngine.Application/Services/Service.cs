using System;
using System.Threading.Tasks;
using CmsEngine.Application.ViewModels;
using CmsEngine.Core.Constants;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class Service : IService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        protected readonly IUnitOfWork unitOfWork;
        protected readonly ILogger logger;

        public InstanceViewModel Instance
        {
            get
            {
                return GetInstanceAsync().GetAwaiter().GetResult();
            }
        }
        public UserViewModel CurrentUser
        {
            get
            {
                return GetCurrentUserViewModelAsync().GetAwaiter().GetResult();
            }
        }

        public Service(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
        {
            unitOfWork = uow ?? throw new ArgumentNullException("Service");
            _httpContextAccessor = hca;
            logger = loggerFactory.CreateLogger("Service");
            _memoryCache = memoryCache;
        }

        internal async Task<ApplicationUser> GetCurrentUserAsync()
        {
            logger.LogInformation("GetCurrentUserAsync() for {0}", _httpContextAccessor.HttpContext.User.Identity.Name);

            try
            {
                return await unitOfWork.Users.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error when trying to load CurrentUser");
                throw ex;
            }
        }

        private async Task<InstanceViewModel> GetInstanceAsync()
        {
            logger.LogInformation("GetInstanceAsync()");

            Website website;
            InstanceViewModel instance;

            try
            {
                logger.LogInformation("Loading Instance from cache");
                if (!_memoryCache.TryGetValue(CmsEngineConstants.CacheKey.Instance, out instance))
                {
                    logger.LogInformation("Empty cache for Instance. Loading instance from DB");
                    website = await unitOfWork.Websites.GetWebsiteInstanceByHost(_httpContextAccessor.HttpContext.Request.Host.Host);

                    if (website != null)
                    {
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

                        var timeSpan = TimeSpan.FromDays(7); //TODO: Perhaps set this in the config file. Or DB

                        logger.LogInformation("Adding Instance to cache with expiration date to {0}", DateTime.Now.AddMilliseconds(timeSpan.TotalMilliseconds).ToString());
                        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(timeSpan);
                        _memoryCache.Set(CmsEngineConstants.CacheKey.Instance, instance, cacheEntryOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error when trying to load Instance");
                throw ex;
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
}
