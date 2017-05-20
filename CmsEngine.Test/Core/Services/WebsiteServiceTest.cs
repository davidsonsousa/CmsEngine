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
    public class WebsiteServiceTest
    {
        private WebsiteService moqWebsiteService;

        [TestInitialize]
        public void InitializeTest()
        {
            moqWebsiteService = WebsiteSetup.SetupService();
        }

        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllWebsitesAsQueryable()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().Count;

            // Act
            var response = moqWebsiteService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Website>, "Response is not IQueryable<Website>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetAllReadOnly_ShouldReturnAllWebsitesAsEnumerable()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().Count;

            // Act
            var response = moqWebsiteService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Website>, "Response is not IEnumerable<Website>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqWebsiteService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void GetByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupEditModel_ShouldReturnNewWebsite()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.SetupEditModel();

            // Assert
            Assert.IsNotNull(response, "Item doesn't exist");
            Assert.IsTrue(((WebsiteEditModel)response).IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupEditModel_ById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebsiteService.SetupEditModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(WebsiteEditModel));
            Assert.AreEqual(((WebsiteEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(WebsiteEditModel));
            Assert.AreEqual(((WebsiteEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebsiteService.SetupViewModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(WebsiteViewModel));
            Assert.AreEqual(((WebsiteViewModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(WebsiteViewModel));
            Assert.AreEqual(((WebsiteViewModel)response).Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Website()
        {
            // Arrange

            // Act
            var websiteEditModel = new WebsiteEditModel
            {
                Name = "Website3",
                Culture = "cs-cz"
            };

            var response = moqWebsiteService.Save(websiteEditModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Website_By_Id()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Website_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
