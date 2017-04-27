using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.ViewModels;
using System;
using System.Linq.Expressions;

namespace CmsEngine.Test.Services
{
    [TestClass]
    public class PostServiceTest
    {
        #region Get

        [TestMethod]
        public void Get_All_Posts_Queryable()
        {
            // Arrange
            var moqPostService = this.SetupPostService();
            var expectedResult = ListOfPosts.Count;

            // Act
            var response = moqPostService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Post>, "Response is not IQueryable<Post>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_All_Posts_ReadOnly()
        {
            // Arrange
            var moqPostService = this.SetupPostService();
            var expectedResult = ListOfPosts.Count;

            // Act
            var response = moqPostService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Post>, "Response is not IEnumerable<Post>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_Post_By_Id()
        {
            // Arrange
            var moqPostService = this.SetupPostService();
            var expectedResult = ListOfPosts.FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPostService.GetById(1);

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        [TestMethod]
        public void Get_Post_By_VanityId()
        {
            // Arrange
            var moqPostService = this.SetupPostService();
            var expectedResult = ListOfPosts.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void Setup_Post_ViewModel_Return_New_Item()
        {
            // Arrange
            var moqWebService = this.SetupPostService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Post>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Post>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void Setup_Post_ViewModel_Return_Multiple_Items()
        {
            // Arrange
            var moqWebService = this.SetupPostService();
            var expectedResult = ListOfPosts.Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Post>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void Setup_Post_ViewModel_Get_Item_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupPostService();
            var expectedResult = ListOfPosts.FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Post>)response).Item.Title, expectedResult);
        }

        [TestMethod]
        public void Setup_Post_ViewModel_Get_Item_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupPostService();
            var expectedResult = ListOfPosts.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqWebService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Post>)response).Item.Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Post()
        {
            // Arrange
            var moqWebService = this.SetupPostService();

            // Act
            var post = new Post
            {
                Title = "Post3",
                IsDeleted = false
            };

            var postViewModel = new BaseViewModel<Post>
            {
                Item = post
            };

            var response = moqWebService.Save(postViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Post_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupPostService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Post_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupPostService();

            // Act
            var response = moqWebService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion

        #region Test configuration

        /// <summary>
        /// Returns a list of posts
        /// </summary>
        public List<Post> ListOfPosts
        {
            get
            {
                return new List<Post>
                {
                    new Post { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1", IsDeleted = false },
                    new Post { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", IsDeleted = false }
                };
            }
        }

        private PostService SetupPostService()
        {
            // Setup the values the repository should return
            var moqRepository = new Mock<IRepository<Post>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Post, bool>>>())).Returns(ListOfPosts.AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Post, bool>>>())).Returns(ListOfPosts);

            // Setup our unit of work
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Post>()).Returns(moqRepository.Object);

            return new PostService(moqUnitOfWork.Object);
        }

        #endregion
    }
}
