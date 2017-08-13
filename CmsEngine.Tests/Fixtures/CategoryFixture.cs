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
    public class CategoryFixture : BaseFixture
    {
        private Mock<IRepository<Category>> moqRepository;
        public Mock<IRepository<Category>> MoqRepository
        {
            get { return moqRepository; }
        }

        private Mock<IUnitOfWork> moqUnitOfWork;
        public Mock<IUnitOfWork> MoqUnitOfWork
        {
            get { return moqUnitOfWork; }
        }

        private CategoryService service;
        public CategoryService Service
        {
            get { return service; }
        }

        private Mock<IMapper> moqMapper;
        public Mock<IMapper> MoqMapper
        {
            get { return moqMapper; }
        }

        public CategoryFixture(): base()
        {
            SetupRepository();
            SetupUnitOfWork();
            SetupMapper();

            service = new CategoryService(moqUnitOfWork.Object, moqMapper.Object, MoqHttpContextAccessor.Object);
        }

        /// <summary>
        /// Returns a list of categories
        /// </summary>
        public List<Category> GetTestCategories()
        {
            return new List<Category>
                {
                    new Category { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Category1", Description="Welcome to category 1", IsDeleted = false },
                    new Category { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description="Welcome to category 2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<CategoryViewModel> GetViewModels()
        {
            return new List<CategoryViewModel>
                {
                    new CategoryViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Category1", Description="Welcome to category 1" },
                    new CategoryViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description="Welcome to category 2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public CategoryEditModel GetEditModel()
        {
            return new CategoryEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description = "Welcome to category 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public CategoryViewModel GetViewModel()
        {
            return new CategoryViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description = "Welcome to category 2" };
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            moqRepository = new Mock<IRepository<Category>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Category, bool>>>())).Returns(GetTestCategories().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Category, bool>>>())).Returns(GetTestCategories());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Category>()).Returns(MoqRepository.Object);

            // Website instance
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(MoqInstance.Object);
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupMapper()
        {
            moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Category, CategoryEditModel>(It.IsAny<Category>())).Returns(GetEditModel());
            moqMapper.Setup(x => x.Map<Category, CategoryViewModel>(It.IsAny<Category>())).Returns(GetViewModel());
            moqMapper.Setup(x => x.Map<Category, CategoryViewModel>(null)).Returns<CategoryViewModel>(null);
            moqMapper.Setup(x => x.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(It.IsAny<IEnumerable<Category>>())).Returns(GetViewModels());
        }
    }
}
