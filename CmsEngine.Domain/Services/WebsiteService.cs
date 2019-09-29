using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Domain.Services
{
    public class WebsiteService : Service, IWebsiteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebsiteService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }
    }
}
