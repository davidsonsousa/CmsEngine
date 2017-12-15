using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CmsEngine.Tests
{
    public sealed partial class TestFixture
    {
        private Mock<IRepository<Post>> _moqPostRepository;
        private Mock<IRepository<Page>> _moqPageRepository;
        private Mock<IRepository<Category>> _moqCategoryRepository;
        private Mock<IRepository<Tag>> _moqTagRepository;
        private Mock<IRepository<Website>> _moqWebsiteRepository;
        private Mock<IRepository<Website>> _moqInstance;
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private Mock<IMapper> _moqMapper;
        private Mock<IHttpContextAccessor> _moqHttpContextAccessor;

        private CmsService _service;
        public CmsService Service { get { return _service; } }

        public TestFixture()
        {
            SetupInstance();
            SetupRepository();
            SetupUnitOfWork();
            SetupMappers();
            SetupHttpContextAccessor();

            _service = new CmsService(_moqUnitOfWork.Object, _moqMapper.Object, _moqHttpContextAccessor.Object);
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

        #region Setup

        private void SetupMappers()
        {
            _moqMapper = new Mock<IMapper>();

            SetupPostMapper();
            SetupPageMapper();
            SetupTagMapper();
            SetupCategoryMapper();
            SetupWebsiteMapper();
        }

        /// <summary>
        /// Setup the instance and its returning value
        /// </summary>
        /// <returns></returns>
        private void SetupInstance()
        {
            _moqInstance = new Mock<IRepository<Website>>();
            _moqInstance.Setup(x => x.Get(It.IsAny<Expression<Func<Website, bool>>>(), "")).Returns(GetInstance().AsQueryable());
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupHttpContextAccessor()
        {
            var hostString = new HostString("cmsengine.dev", 5000);

            _moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _moqHttpContextAccessor.Setup(x => x.HttpContext.Request.Host).Returns(hostString);
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            _moqPostRepository = new Mock<IRepository<Post>>();
            _moqPostRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Post, bool>>>(), "")).Returns(GetTestPosts().AsQueryable());
            _moqPostRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Post, bool>>>())).Returns(GetTestPosts());

            _moqPageRepository = new Mock<IRepository<Page>>();
            _moqPageRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Page, bool>>>(), "")).Returns(GetTestPages().AsQueryable());
            _moqPageRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Page, bool>>>())).Returns(GetTestPages());

            _moqTagRepository = new Mock<IRepository<Tag>>();
            _moqTagRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Tag, bool>>>(), "")).Returns(GetTestTags().AsQueryable());
            _moqTagRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(GetTestTags());

            _moqCategoryRepository = new Mock<IRepository<Category>>();
            _moqCategoryRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Category, bool>>>(), "")).Returns(GetTestCategories().AsQueryable());
            _moqCategoryRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Category, bool>>>())).Returns(GetTestCategories());

            _moqWebsiteRepository = new Mock<IRepository<Website>>();
            _moqWebsiteRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Website, bool>>>(), "")).Returns(GetTestWebsites().AsQueryable());
            _moqWebsiteRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Website, bool>>>())).Returns(GetTestWebsites());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>();
            _moqUnitOfWork.Setup(x => x.Posts).Returns(_moqPostRepository.Object);
            _moqUnitOfWork.Setup(x => x.Pages).Returns(_moqPageRepository.Object);
            _moqUnitOfWork.Setup(x => x.Tags).Returns(_moqTagRepository.Object);
            _moqUnitOfWork.Setup(x => x.Categories).Returns(_moqCategoryRepository.Object);
            _moqUnitOfWork.Setup(x => x.Websites).Returns(_moqWebsiteRepository.Object);

            _moqUnitOfWork.Setup(x => x.GetRepository<Post>()).Returns(_moqPostRepository.Object);
            _moqUnitOfWork.Setup(x => x.GetRepository<Page>()).Returns(_moqPageRepository.Object);
            _moqUnitOfWork.Setup(x => x.GetRepository<Category>()).Returns(_moqCategoryRepository.Object);
            _moqUnitOfWork.Setup(x => x.GetRepository<Tag>()).Returns(_moqTagRepository.Object);
            _moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(_moqWebsiteRepository.Object);

            // Website instance
            _moqUnitOfWork.Setup(x => x.Websites).Returns(_moqInstance.Object);
        }

        #endregion
    }
}
