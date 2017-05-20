using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CmsEngine.Test.Setup
{
    public static class CategorySetup
    {
        /// <summary>
        /// Returns a list of tags
        /// </summary>
        public static List<Category> GetTestCategories()
        {
            return new List<Category>
                {
                    new Category { Id = 1, VanityId = new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"), Name = "Category1", IsDeleted = false },
                    new Category { Id = 2, VanityId = new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"), Name = "Category2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Setup the Repository and its returning values
        /// </summary>
        /// <returns></returns>
        public static Mock<IRepository<Category>> SetupCategoryRepository()
        {
            var moqRepository = new Mock<IRepository<Category>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Category, bool>>>())).Returns(GetTestCategories().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Category, bool>>>())).Returns(GetTestCategories());

            return moqRepository;
        }

        /// <summary>
        /// Setup the Unit Of Work
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        public static Mock<IUnitOfWork> SetupUnitOfWork(IMock<IRepository<Category>> moqRepository)
        {
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Category>()).Returns(moqRepository.Object);

            return moqUnitOfWork;
        }

        /// <summary>
        /// Setup CategoryService
        /// </summary>
        /// <param name="moqUnitOfWork"></param>
        /// <returns></returns>
        public static CategoryService SetupService()
        {
            var moqRepository = SetupCategoryRepository();
            var moqUnitOfWork = SetupUnitOfWork(moqRepository);

            return new CategoryService(moqUnitOfWork.Object);
        }
    }
}
