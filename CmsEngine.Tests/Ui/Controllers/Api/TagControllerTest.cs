using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Tests.Fixtures;
using CmsEngine.Ui.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CmsEngine.Tests.Ui.Controllers.Api
{
    public class TagControllerTest : IClassFixture<TagFixture>
    {
        private Mock<IRepository<Tag>> moqRepository;

        private TagFixture tagFixture;
        private TagController controller;

        public TagControllerTest(TagFixture fixture)
        {
            tagFixture = fixture;
            moqRepository = tagFixture.MoqRepository;

            controller = new TagController(tagFixture.MoqUnitOfWork.Object, tagFixture.MoqMapper.Object, tagFixture.MoqHttpContextAccessor.Object);
        }

        [Fact]
        public void GetAllTags_ShouldReturnAllTags()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags();

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<TagViewModel>;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Count(), testResult.Count());
        }

        [Fact]
        public void GetTagById_ShouldReturnSelectedTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.Id == 2);

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as TagViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetTagById_ShouldReturnNotFound()
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
        public void GetTagByVanityId_ShouldReturnSelectedTag()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.VanityId == id);

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as TagViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetTagByVanityId_ShouldReturnNotFound()
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
        public void PostTag_ShouldReturnCreated()
        {
            // Arrange
            var tagEditModel = new TagEditModel
            {
                Name = "Post Tag",
                Slug = "post-tag"
            };

            moqRepository.Setup(x => x.Insert(It.IsAny<Tag>())).Verifiable();

            // Act
            var actionResult = controller.Post(tagEditModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(createdResult);
        }

        [Fact]
        public void PostTag_ShouldReturnBadRequest()
        {
            // Arrange
            var tagEditModel = new TagEditModel
            {
                Slug = "post-tag",
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Post(tagEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void PutTag_ShouldReturnOk()
        {
            // Arrange
            var tagId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var tagEditModel = new TagEditModel
            {
                Id = 1,
                VanityId = tagId,
                Name = "Put Tag",
                Slug = "put-tag"
            };

            moqRepository.Setup(x => x.Update(It.IsAny<Tag>())).Verifiable();

            // Act
            var actionResult = controller.Put(tagId, tagEditModel);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public void PutTag_ShouldReturnBadRequest()
        {
            // Arrange
            var tagId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var tagEditModel = new TagEditModel
            {
                Id = 1,
                VanityId = tagId,
                Slug = "put-tag"
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Put(tagId, tagEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void DeleteTag_ShouldReturnOk()
        {
            // Arrange
            var tagId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");

            moqRepository.Setup(x => x.Update(It.IsAny<Tag>())).Verifiable();

            // Act
            var actionResult = controller.Delete(tagId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
