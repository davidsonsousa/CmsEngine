using System;
using System.Threading.Tasks;
using CmsEngine.Data;
using CmsEngine.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Domain.Services
{
    public class Service : IService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly IUnitOfWork unitOfWork;
        protected readonly ILogger logger;

        public InstanceViewModel Instance { get; private set; }
        public UserViewModel CurrentUser { get; private set; }

        public Service(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log)
        {
            unitOfWork = uow ?? throw new ArgumentNullException("Service");
            _httpContextAccessor = hca;
            logger = log;

            GetInstance().ConfigureAwait(false);
        }

        private async Task GetInstance()
        {
            if (Instance == null)
            {
                var website = await unitOfWork.Websites.GetWebsiteInstanceByHost(_httpContextAccessor.HttpContext.Request.Host.Host);

                if (website != null)
                {
                    Instance = new InstanceViewModel
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
                        }
                    };
                }
            }
        }

        private async Task<UserViewModel> GetCurrentUser()
        {
            var user = await unitOfWork.Users.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

            return new UserViewModel
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
