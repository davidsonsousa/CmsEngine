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
    public class WebsiteControllerTest : IClassFixture<WebsiteFixture>
    {
        private Mock<IRepository<Website>> moqRepository;

        private WebsiteFixture websiteFixture;
        private WebsiteController controller;

        public WebsiteControllerTest(WebsiteFixture fixture)
        {
            websiteFixture = fixture;
            moqRepository = websiteFixture.MoqRepository;

            controller = new WebsiteController(websiteFixture.MoqUnitOfWork.Object, websiteFixture.MoqMapper.Object, websiteFixture.MoqHttpContextAccessor.Object);
        }

        [Fact]
        public void GetAllWebsites_ShouldReturnAllWebsites()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites();

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<WebsiteViewModel>;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Count(), testResult.Count());
        }

        [Fact]
        public void GetWebsiteById_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.Id == 2);

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as WebsiteViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetWebsiteById_ShouldReturnNotFound()
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
        public void GetWebsiteByVanityId_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.VanityId == id);

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as WebsiteViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetWebsiteByVanityId_ShouldReturnNotFound()
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
        public void PostWebsite_ShouldReturnCreated()
        {
            // Arrange
            var websiteEditModel = new WebsiteEditModel
            {
                Name = "Post Website",
                Culture = "en-US",
                Description = "Welcome to the post test website"
            };

            moqRepository.Setup(x => x.Insert(It.IsAny<Website>())).Verifiable();

            // Act
            var actionResult = controller.Post(websiteEditModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(createdResult);
        }

        [Fact]
        public void PostWebsite_ShouldReturnBadRequest()
        {
            // Arrange
            var websiteEditModel = new WebsiteEditModel
            {
                Culture = "en-US",
                Description = "Welcome to the post test website"
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Post(websiteEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void PutWebsite_ShouldReturnOk()
        {
            // Arrange
            var websiteId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var websiteEditModel = new WebsiteEditModel
            {
                Id = 1,
                VanityId = websiteId,
                Name = "Put Website",
                Culture = "en-US",
                Description = "Welcome to the put test website"
            };

            moqRepository.Setup(x => x.Update(It.IsAny<Website>())).Verifiable();

            // Act
            var actionResult = controller.Put(websiteId, websiteEditModel);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public void PutWebsite_ShouldReturnBadRequest()
        {
            // Arrange
            var websiteId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var websiteEditModel = new WebsiteEditModel
            {
                Id = 1,
                VanityId = websiteId,
                Culture = "en-US",
                Description = "Welcome to the put test website"
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Put(websiteId, websiteEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void DeleteWebsite_ShouldReturnOk()
        {
            // Arrange
            var websiteId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");

            moqRepository.Setup(x => x.Update(It.IsAny<Website>())).Verifiable();

            // Act
            var actionResult = controller.Delete(websiteId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
