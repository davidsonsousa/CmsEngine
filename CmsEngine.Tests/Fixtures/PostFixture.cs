using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using Moq;

namespace CmsEngine.Tests.Fixtures
{
    public class PostFixture : BaseFixture
    {
        private Mock<IRepository<Post>> moqRepository;
        public Mock<IRepository<Post>> MoqRepository
        {
            get { return moqRepository; }
        }

        private Mock<IUnitOfWork> moqUnitOfWork;
        public Mock<IUnitOfWork> MoqUnitOfWork
        {
            get { return moqUnitOfWork; }
        }

        private CmsService service;
        public CmsService Service
        {
            get { return service; }
        }

        private Mock<IMapper> moqMapper;
        public Mock<IMapper> MoqMapper
        {
            get { return moqMapper; }
        }

        public PostFixture() : base()
        {
            SetupRepository();
            SetupUnitOfWork();
            SetupMapper();

            service = new CmsService(moqUnitOfWork.Object, moqMapper.Object, MoqHttpContextAccessor.Object);
        }


        /// <summary>
        /// Returns a list of posts
        /// </summary>
        public List<Post> GetTestPosts()
        {
            return new List<Post>
                {
                    new Post { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1", IsDeleted = false },
                    new Post { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<PostViewModel> GetViewModels()
        {
            return new List<PostViewModel>
                {
                    new PostViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1" },
                    new PostViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PostEditModel GetEditModel()
        {
            return new PostEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PostViewModel GetViewModel()
        {
            return new PostViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            moqRepository = new Mock<IRepository<Post>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Post, bool>>>(), "")).Returns(GetTestPosts().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Post, bool>>>())).Returns(GetTestPosts());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Post>()).Returns(MoqRepository.Object);

            // Website instance
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(MoqInstance.Object);
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupMapper()
        {
            moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Post, PostEditModel>(It.IsAny<Post>())).Returns(GetEditModel());
            moqMapper.Setup(x => x.Map<Post, PostViewModel>(It.IsAny<Post>())).Returns(GetViewModel());
            moqMapper.Setup(x => x.Map<Post, PostViewModel>(null)).Returns<PostViewModel>(null);
            moqMapper.Setup(x => x.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(It.IsAny<IEnumerable<Post>>())).Returns(GetViewModels());
        }
    }
}
