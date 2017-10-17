using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Tests.Fixtures;
using CmsEngine.Ui.Angular.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CmsEngine.Tests.Ui.Angular.Controllers.Api
{
    public class PageControllerTest : IClassFixture<PageFixture>
    {
        private Mock<IRepository<Page>> moqRepository;

        private PageFixture pageFixture;
        private PageController controller;

        public PageControllerTest(PageFixture fixture)
        {
            pageFixture = fixture;
            moqRepository = pageFixture.MoqRepository;

            controller = new PageController(pageFixture.MoqUnitOfWork.Object, pageFixture.MoqMapper.Object, pageFixture.MoqHttpContextAccessor.Object);
        }

        [Fact]
        public void GetAllPages_ShouldReturnAllPages()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages();

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<PageViewModel>;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Count(), testResult.Count());
        }

        [Fact]
        public void GetPageById_ShouldReturnSelectedPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 2);

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as PageViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Title, testResult.Title);
        }

        [Fact]
        public void GetPageById_ShouldReturnNotFound()
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
        public void GetPageByVanityId_ShouldReturnSelectedPage()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == id);

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as PageViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Title, testResult.Title);
        }

        [Fact]
        public void GetPageByVanityId_ShouldReturnNotFound()
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
        public void PostPage_ShouldReturnCreated()
        {
            // Arrange
            var pageEditModel = new PageEditModel
            {
                Title = "Post Page",
                Description = "Welcome to the post test page",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "post-page",
                PublishedOn = DateTime.Now
            };

            moqRepository.Setup(x => x.Insert(It.IsAny<Page>())).Verifiable();

            // Act
            var actionResult = controller.Post(pageEditModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(createdResult);
        }

        [Fact]
        public void PostPage_ShouldReturnBadRequest()
        {
            // Arrange
            var pageEditModel = new PageEditModel
            {
                Description = "Welcome to the post test page",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "post-page",
                PublishedOn = DateTime.Now
            };

            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var actionResult = controller.Post(pageEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void PutPage_ShouldReturnOk()
        {
            // Arrange
            var pageId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var pageEditModel = new PageEditModel
            {
                Id = 1,
                VanityId = pageId,
                Title = "Put Page",
                Description = "Welcome to the put test page",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "put-page",
                PublishedOn = DateTime.Now
            };

            moqRepository.Setup(x => x.Update(It.IsAny<Page>())).Verifiable();

            // Act
            var actionResult = controller.Put(pageId, pageEditModel);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public void PutPage_ShouldReturnBadRequest()
        {
            // Arrange
            var pageId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var pageEditModel = new PageEditModel
            {
                Id = 1,
                VanityId = pageId,
                Description = "Welcome to the put test page",
                Author = "John Doe",
                DocumentContent = "Hello world",
                Slug = "put-page",
                PublishedOn = DateTime.Now
            };

            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var actionResult = controller.Put(pageId, pageEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void DeletePage_ShouldReturnOk()
        {
            // Arrange
            var pageId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");

            moqRepository.Setup(x => x.Update(It.IsAny<Page>())).Verifiable();

            // Act
            var actionResult = controller.Delete(pageId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
