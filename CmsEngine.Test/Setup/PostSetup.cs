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
    public static class PostSetup
    {
        /// <summary>
        /// Returns a list of posts
        /// </summary>
        public static List<Post> GetTestPosts()
        {
            return new List<Post>
                {
                    new Post { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1", IsDeleted = false },
                    new Post { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Setup the Repository and its returning values
        /// </summary>
        /// <returns></returns>
        public static Mock<IRepository<Post>> SetupPostRepository()
        {
            var moqRepository = new Mock<IRepository<Post>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Post, bool>>>())).Returns(GetTestPosts().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Post, bool>>>())).Returns(GetTestPosts());

            return moqRepository;
        }

        /// <summary>
        /// Setup the Unit Of Work
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        public static Mock<IUnitOfWork> SetupUnitOfWork(IMock<IRepository<Post>> moqRepository)
        {
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Post>()).Returns(moqRepository.Object);

            return moqUnitOfWork;
        }

        /// <summary>
        /// Setup PostService
        /// </summary>
        /// <param name="moqUnitOfWork"></param>
        /// <returns></returns>
        public static PostService SetupService()
        {
            var moqRepository = SetupPostRepository();
            var moqUnitOfWork = SetupUnitOfWork(moqRepository);

            return new PostService(moqUnitOfWork.Object);
        }
    }
}
