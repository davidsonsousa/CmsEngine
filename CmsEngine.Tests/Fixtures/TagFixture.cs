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
    public class TagFixture : BaseFixture
    {
        private Mock<IRepository<Tag>> moqRepository;
        public Mock<IRepository<Tag>> MoqRepository
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

        public TagFixture() : base()
        {
            SetupRepository();
            SetupUnitOfWork();
            SetupMapper();

            service = new CmsService(moqUnitOfWork.Object, moqMapper.Object, MoqHttpContextAccessor.Object);
        }

        /// <summary>
        /// Returns a list of tags
        /// </summary>
        public List<Tag> GetTestTags()
        {
            return new List<Tag>
                {
                    new Tag { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1", IsDeleted = false },
                    new Tag { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<TagViewModel> GetViewModels()
        {
            return new List<TagViewModel>
                {
                    new TagViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1" },
                    new TagViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public TagEditModel GetEditModel()
        {
            return new TagEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public TagViewModel GetViewModel()
        {
            return new TagViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" };
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            moqRepository = new Mock<IRepository<Tag>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(GetTestTags().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(GetTestTags());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Tag>()).Returns(MoqRepository.Object);

            // Website instance
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(MoqInstance.Object);
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupMapper()
        {
            moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Tag, TagEditModel>(It.IsAny<Tag>())).Returns(GetEditModel());
            moqMapper.Setup(x => x.Map<Tag, TagViewModel>(It.IsAny<Tag>())).Returns(GetViewModel());
            moqMapper.Setup(x => x.Map<Tag, TagViewModel>(null)).Returns<TagViewModel>(null);
            moqMapper.Setup(x => x.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(It.IsAny<IEnumerable<Tag>>())).Returns(GetViewModels());
        }
    }
}
