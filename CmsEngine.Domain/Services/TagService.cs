using CmsEngine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Domain.Services
{
    public class TagService : Service, ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }
    }
}
