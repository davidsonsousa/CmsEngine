using CmsEngine.Api.Controllers;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Test.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

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
            var response = controller.Get();

            // Assert
            Assert.IsTrue(response.SequenceEqual(expectedResult), "Response doesn't match expected result");
        }

        [TestMethod]
        public void GetWebsiteById_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = controller.Get(1);

            // Assert
            Assert.AreEqual(response, expectedResult);
        }

        [TestMethod]
        public void GetWebsiteByVanityId_ShouldReturnSelectedWebsite()
        {
            // Arrange
            var vanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == vanityId).Name;

            // Act
            var response = controller.Get(vanityId);

            // Assert
            Assert.AreEqual(response, expectedResult);
        }
    }
}
