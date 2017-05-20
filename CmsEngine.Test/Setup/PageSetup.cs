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
    public static class PageSetup
    {
        /// <summary>
        /// Returns a list of pages
        /// </summary>
        public static List<Page> GetTestPages()
        {
            return new List<Page>
                {
                    new Page { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Page1", IsDeleted = false },
                    new Page { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Setup the Repository and its returning values
        /// </summary>
        /// <returns></returns>
        public static Mock<IRepository<Page>> SetupPageRepository()
        {
            var moqRepository = new Mock<IRepository<Page>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Page, bool>>>())).Returns(GetTestPages().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Page, bool>>>())).Returns(GetTestPages());

            return moqRepository;
        }

        /// <summary>
        /// Setup the Unit Of Work
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        public static Mock<IUnitOfWork> SetupUnitOfWork(IMock<IRepository<Page>> moqRepository)
        {
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Page>()).Returns(moqRepository.Object);

            return moqUnitOfWork;
        }

        /// <summary>
        /// Setup PageService
        /// </summary>
        /// <param name="moqUnitOfWork"></param>
        /// <returns></returns>
        public static PageService SetupService()
        {
            var moqRepository = SetupPageRepository();
            var moqUnitOfWork = SetupUnitOfWork(moqRepository);

            return new PageService(moqUnitOfWork.Object);
        }
    }
}
