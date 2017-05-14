using CmsEngine.Api.Controllers;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Test.Setup;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CmsEngine.Data.EditModels;

namespace CmsEngine.Test.Api.Controllers
{
    [TestClass]
    public class WebsiteControllerTest
    {
        private Mock<IRepository<Website>> moqRepository;
        private Mock<IUnitOfWork> moqUnitOfWork;
        private WebsiteController controller;

        [TestInitialize]
        public void InitializeTest()
        {
            moqRepository = WebsiteSetup.SetupWebsiteRepository();
            moqUnitOfWork = WebsiteSetup.SetupUnitOfWork(moqRepository);
            controller = new WebsiteController(moqUnitOfWork.Object);
        }

        [TestMethod]
        public void GetAllWebsites_ShouldReturnAllWebsites()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().Select(x => x.Name);

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<string>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.IsTrue(testResult.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void GetWebsiteById_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as string;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(testResult, expectedResult);
        }

        [TestMethod]
        public void GetWebsiteById_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var actionResult = controller.Get(10);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void GetWebsiteByVanityId_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == id).Name;

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as string;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(testResult, expectedResult);
        }

        [TestMethod]
        public void GetWebsiteByVanityId_ShouldReturnNotFound()
        {
            // Arrange
            var id = new Guid("41ec584b-6d8f-4110-aef4-f9a5036b9bff");

            // Act
            var actionResult = controller.Get(id);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void PostWebsite_ShouldReturnCreated()
        {
            // Arrange
            var websiteViewModel = new WebsiteEditModel
            {
                Name = "Post Website",
                Culture = "en-US",
                Description = "Welcome to the post test website"
            };

            // Act
            var actionResult = controller.Post(websiteViewModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual($"Website '{websiteViewModel.Name}' saved.", ((ReturnValue)createdResult.RouteValues["returnValue"]).Message);
        }

        [TestMethod]
        public void PostWebsite_ShouldReturnBadRequest()
        {
            // Arrange
            var websiteViewModel = new WebsiteEditModel
            {
                Culture = "en-US",
                Description = "Welcome to the post test website"
            };

            // Act
            var actionResult = controller.Post(websiteViewModel);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }
    }
}
