using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.ViewModels;
using CmsEngine.Tests.Fixtures;
using Xunit;

namespace CmsEngine.Test.Core.Services
{
    public class PostServiceTest : IClassFixture<PostFixture>
    {
        private PostFixture postFixture;
        private CmsService moqPostService;

        public PostServiceTest(PostFixture fixture)
        {
            postFixture = fixture;
            moqPostService = postFixture.Service;
        }

        #region Get

        //[Fact]
        //public void GetAll_ShouldReturnAllPostsAsQueryable()
        //{
        //    // Arrange
        //    var expectedResult = postFixture.GetTestPosts().Count;

        //    // Act
        //    var response = moqPostService.GetAll();

        //    // Assert
        //    Assert.True(response is IQueryable<Post>, "Response is not IQueryable<Post>");
        //    Assert.Equal(expectedResult, response.Count());
        //}

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllPostsAsEnumerable()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().Count;

            // Act
            var response = (IEnumerable<PostViewModel>)moqPostService.GetAllPostsReadOnly();

            // Assert
            Assert.True(response is IEnumerable<PostViewModel>, "Response is not IEnumerable<PostViewModel>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PostViewModel)moqPostService.GetPostById(2);

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PostViewModel)moqPostService.GetPostById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

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
            var response = (PostEditModel)moqPostService.SetupPostEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PostEditModel)moqPostService.SetupPostEditModel(2);

            // Assert
            Assert.IsType(typeof(PostEditModel), response);
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PostEditModel)moqPostService.SetupPostEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PostEditModel), response);
            Assert.Equal(expectedResult, response.Title);
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

            var response = moqPostService.SavePost(postEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Post_By_Id()
        {
            // Arrange

            // Act
            var response = moqPostService.DeletePost(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Post_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPostService.DeletePost(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
