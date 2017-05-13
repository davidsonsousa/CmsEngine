using CmsEngine.Api.Controllers;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Test.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var contentResult = actionResult as OkObjectResult;
            var testResult = contentResult.Value as IEnumerable<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(200, contentResult.StatusCode);
            Assert.IsTrue(testResult.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void GetWebsiteById_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var actionResult = controller.Get(1);
            var contentResult = actionResult as OkObjectResult;
            var testResult = contentResult.Value as string;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(200, contentResult.StatusCode);
            Assert.AreEqual(testResult, expectedResult);
        }

        [TestMethod]
        public void GetWebsiteById_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var actionResult = controller.Get(10);
            var contentResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(404, contentResult.StatusCode);
        }

        [TestMethod]
        public void GetWebsiteByVanityId_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var vanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == vanityId).Name;

            // Act
            var actionResult = controller.Get(vanityId);
            var contentResult = actionResult as OkObjectResult;
            var testResult = contentResult.Value as string;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(200, contentResult.StatusCode);
            Assert.AreEqual(testResult, expectedResult);
        }

        [TestMethod]
        public void GetWebsiteByVanityId_ShouldReturnNotFound()
        {
            // Arrange
            var vanityId = new Guid("41ec584b-6d8f-4110-aef4-f9a5036b9bff");

            // Act
            var actionResult = controller.Get(vanityId);
            var contentResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(404, contentResult.StatusCode);
        }
    }
}
