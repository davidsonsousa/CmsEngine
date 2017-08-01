using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using CmsEngine.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CmsEngine.Test.Core.Services
{
    public class PostServiceTest : IClassFixture<PostFixture>
    {
        private PostFixture postFixture;
        private PostService moqPostService;

        public PostServiceTest(PostFixture fixture)
        {
            postFixture = fixture;
            moqPostService = postFixture.Service;
        }

        #region Get

        [Fact]
        public void GetAll_ShouldReturnAllPostsAsQueryable()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().Count;

            // Act
            var response = moqPostService.GetAll();

            // Assert
            Assert.True(response is IQueryable<Post>, "Response is not IQueryable<Post>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllPostsAsEnumerable()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().Count;

            // Act
            var response = moqPostService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<Post>, "Response is not IEnumerable<Post>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPostService.GetById(1);

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewPost()
        {
            // Arrange

            // Act
            var response = moqPostService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(((PostEditModel)response).IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPostService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(PostEditModel), response);
            Assert.Equal(expectedResult, ((PostEditModel)response).Title);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PostEditModel), response);
            Assert.Equal(expectedResult, ((PostEditModel)response).Title);
        }

        [Fact]
        public void SetupViewModel_ById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPostService.SetupViewModel(2);

            // Assert
            Assert.IsType(typeof(PostViewModel), response);
            Assert.Equal(expectedResult, ((PostViewModel)response).Title);
        }

        [Fact]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPostService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PostViewModel), response);
            Assert.Equal(expectedResult, ((PostViewModel)response).Title);
        }

        #endregion

        #region DB Changes

        [Fact]
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
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Post_By_Id()
        {
            // Arrange

            // Act
            var response = moqPostService.Delete(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Post_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPostService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
