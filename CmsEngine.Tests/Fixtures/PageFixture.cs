using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CmsEngine.Tests.Fixtures
{
    public class PageFixture
    {
        private Mock<IRepository<Page>> moqRepository;
        public Mock<IRepository<Page>> MoqRepository
        {
            get { return moqRepository; }
        }

        private Mock<IUnitOfWork> moqUnitOfWork;
        public Mock<IUnitOfWork> MoqUnitOfWork
        {
            get { return moqUnitOfWork; }
        }

        private PageService service;
        public PageService Service
        {
            get { return service; }
        }

        private Mock<IMapper> moqMapper;
        public Mock<IMapper> MoqMapper
        {
            get { return moqMapper; }
        }

        public PageFixture()
        {
            SetupRepository();
            SetupUnitOfWork();
            SetupMapper();

            service = new PageService(moqUnitOfWork.Object, moqMapper.Object);
        }


        /// <summary>
        /// Returns a list of pages
        /// </summary>
        public List<Page> GetTestPages()
        {
            return new List<Page>
                {
                    new Page { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Page1", IsDeleted = false },
                    new Page { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PageEditModel GetEditModel()
        {
            return new PageEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PageViewModel GetViewModel()
        {
            return new PageViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            moqRepository = new Mock<IRepository<Page>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Page, bool>>>())).Returns(GetTestPages().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Page, bool>>>())).Returns(GetTestPages());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Page>()).Returns(MoqRepository.Object);
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupMapper()
        {
            moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Page, PageEditModel>(It.IsAny<Page>())).Returns(GetEditModel());
            moqMapper.Setup(x => x.Map<Page, PageViewModel>(It.IsAny<Page>())).Returns(GetViewModel());
        }
    }
}
