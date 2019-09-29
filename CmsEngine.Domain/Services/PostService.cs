using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Domain.Services
{
    public class PostService : Service, IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }
    }
}
