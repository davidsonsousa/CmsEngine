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
    public static class TagSetup
    {
        /// <summary>
        /// Returns a list of tags
        /// </summary>
        public static List<Tag> GetTestTags()
        {
            return new List<Tag>
                {
                    new Tag { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1", IsDeleted = false },
                    new Tag { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Setup the Repository and its returning values
        /// </summary>
        /// <returns></returns>
        public static Mock<IRepository<Tag>> SetupTagRepository()
        {
            var moqRepository = new Mock<IRepository<Tag>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(GetTestTags().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(GetTestTags());

            return moqRepository;
        }

        /// <summary>
        /// Setup the Unit Of Work
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        public static Mock<IUnitOfWork> SetupUnitOfWork(IMock<IRepository<Tag>> moqRepository)
        {
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Tag>()).Returns(moqRepository.Object);

            return moqUnitOfWork;
        }

        /// <summary>
        /// Setup TagService
        /// </summary>
        /// <param name="moqUnitOfWork"></param>
        /// <returns></returns>
        public static TagService SetupService()
        {
            var moqRepository = SetupTagRepository();
            var moqUnitOfWork = SetupUnitOfWork(moqRepository);

            return new TagService(moqUnitOfWork.Object);
        }
    }
}
