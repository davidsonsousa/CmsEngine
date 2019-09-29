using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Domain.Services
{
    public class PageService : Service, IPageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PageService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }
    }
}
