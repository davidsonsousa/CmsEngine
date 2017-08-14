using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Tests.Fixtures;
using CmsEngine.Ui.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace CmsEngine.Tests.Ui.Controllers.Api
{
    public class PostControllerTest : IClassFixture<PostFixture>
    {
        private Mock<IRepository<Post>> moqRepository;

        private PostFixture postFixture;
        private PostController controller;

        public PostControllerTest(PostFixture fixture)
        {
            postFixture = fixture;
            moqRepository = postFixture.MoqRepository;

            controller = new PostController(postFixture.MoqUnitOfWork.Object, postFixture.MoqMapper.Object, postFixture.MoqHttpContextAccessor.Object);
        }

        [Fact]
        public void GetAllPosts_ShouldReturnAllPosts()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts();

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<PostViewModel>;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Count(), testResult.Count());
        }

        [Fact]
        public void GetPostById_ShouldReturnSelectedPost()
        {
            // Arrange
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.Id == 2);

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as PostViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Title, testResult.Title);
        }

        [Fact]
        public void GetPostById_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var actionResult = controller.Get(10);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetPostByVanityId_ShouldReturnSelectedPost()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = postFixture.GetTestPosts().FirstOrDefault(q => q.VanityId == id);

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as PostViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Title, testResult.Title);
        }

        [Fact]
        public void GetPostByVanityId_ShouldReturnNotFound()
        {
            // Arrange
            var id = new Guid("41ec584b-6d8f-4110-aef4-f9a5036b9bff");

            // Act
            var actionResult = controller.Get(id);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void PostPost_ShouldReturnCreated()
        {
            // Arrange
            var postEditModel = new PostEditModel
            {
                Title = "Post Post",
                Description = "Welcome to the post test post",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "post-post",
                PublishedOn = DateTime.Now
            };

            moqRepository.Setup(x => x.Insert(It.IsAny<Post>())).Verifiable();

            // Act
            var actionResult = controller.Post(postEditModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(createdResult);
        }

        [Fact]
        public void PostPost_ShouldReturnBadRequest()
        {
            // Arrange
            var postEditModel = new PostEditModel
            {
                Description = "Welcome to the post test post",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "post-post",
                PublishedOn = DateTime.Now
            };

            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var actionResult = controller.Post(postEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void PutPost_ShouldReturnOk()
        {
            // Arrange
            var postId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var postEditModel = new PostEditModel
            {
                Id = 1,
                VanityId = postId,
                Title = "Put Post",
                Description = "Welcome to the put test post",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "put-post",
                PublishedOn = DateTime.Now
            };

            moqRepository.Setup(x => x.Update(It.IsAny<Post>())).Verifiable();

            // Act
            var actionResult = controller.Put(postId, postEditModel);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public void PutPost_ShouldReturnBadRequest()
        {
            // Arrange
            var postId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var postEditModel = new PostEditModel
            {
                Id = 1,
                VanityId = postId,
                Description = "Welcome to the put test post",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "put-post",
                PublishedOn = DateTime.Now
            };

            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var actionResult = controller.Put(postId, postEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void DeletePost_ShouldReturnOk()
        {
            // Arrange
            var postId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");

            moqRepository.Setup(x => x.Update(It.IsAny<Post>())).Verifiable();

            // Act
            var actionResult = controller.Delete(postId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
