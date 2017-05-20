using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using CmsEngine.Test.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CmsEngine.Test.Core.Services
{
    [TestClass]
    public class PostServiceTest
    {
        private PostService moqPostService;

        [TestInitialize]
        public void InitializeTest()
        {
            moqPostService = PostSetup.SetupService();
        }

        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllPostsAsQueryable()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().Count;

            // Act
            var response = moqPostService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Post>, "Response is not IQueryable<Post>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetAllReadOnly_ShouldReturnAllPostsAsEnumerable()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().Count;

            // Act
            var response = moqPostService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Post>, "Response is not IEnumerable<Post>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPostService.GetById(1);

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        [TestMethod]
        public void GetByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupEditModel_ShouldReturnNewPost()
        {
            // Arrange

            // Act
            var response = moqPostService.SetupEditModel();

            // Assert
            Assert.IsNotNull(response, "Item doesn't exist");
            Assert.IsTrue(((PostEditModel)response).IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupEditModel_ById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPostService.SetupEditModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(PostEditModel));
            Assert.AreEqual(((PostEditModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(PostEditModel));
            Assert.AreEqual(((PostEditModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPostService.SetupViewModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(PostViewModel));
            Assert.AreEqual(((PostViewModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = PostSetup.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(PostViewModel));
            Assert.AreEqual(((PostViewModel)response).Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Post()
        {
            // Arrange

            // Act
            var postEditModel = new PostEditModel
            {
                Title = "Post3"
            };

            var response = moqPostService.Save(postEditModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Post_By_Id()
        {
            // Arrange

            // Act
            var response = moqPostService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Post_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPostService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
