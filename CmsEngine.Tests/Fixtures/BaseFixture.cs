using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CmsEngine.Tests.Fixtures
{
    public class BaseFixture
    {
        private Mock<IRepository<Website>> moqInstance;
        public Mock<IRepository<Website>> MoqInstance
        {
            get { return moqInstance; }
        }

        private Mock<IHttpContextAccessor> moqHttpContextAccessor;
        public Mock<IHttpContextAccessor> MoqHttpContextAccessor
        {
            get { return moqHttpContextAccessor; }
        }

        public BaseFixture()
        {
            SetupInstance();
            SetupHttpContextAccessor();
        }

        /// <summary>
        /// Returns the instance
        /// </summary>
        public List<Website> GetInstance()
        {
            return new List<Website>
            {
                new Website
                {
                    Id = 1,
                    VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"),
                    Name = "CmsEngine Test Instance",
                    Culture = "en-US",
                    Description = "Welcome to the test instance",
                    SiteUrl = "cmsengine.dev",
                    IsDeleted = false
                }
            };
        }

        /// <summary>
        /// Setup the instance and its returning value
        /// </summary>
        /// <returns></returns>
        private void SetupInstance()
        {
            moqInstance = new Mock<IRepository<Website>>();
            moqInstance.Setup(x => x.Get(It.IsAny<Expression<Func<Website, bool>>>(), "")).Returns(GetInstance().AsQueryable());
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupHttpContextAccessor()
        {
            var hostString = new HostString("cmsengine.dev", 5000);

            moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            moqHttpContextAccessor.Setup(x => x.HttpContext.Request.Host).Returns(hostString);
        }
    }
}
