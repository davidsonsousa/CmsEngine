using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.ViewModels;
using System;
using System.Linq.Expressions;
using CmsEngine.Test.Setup;

namespace CmsEngine.Test.Core.Services
{
    [TestClass]
    public class WebsiteServiceTest
    {
        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllWebsitesAsQueryable()
        {
            // Arrange
            var moqWebsiteService = WebsiteSetup.SetupService();
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
            var moqWebsiteService = WebsiteSetup.SetupService();
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
            var moqWebsiteService = WebsiteSetup.SetupService();
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
            var moqWebsiteService = WebsiteSetup.SetupService();
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupViewModel_ShouldReturnNewWebsite()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Website>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Website>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupViewModel_ShouldReturnAllWebsites()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();
            var expectedResult = WebsiteSetup.GetTestWebsites().Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Website>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Website>)response).Item.Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();
            var expectedResult = WebsiteSetup.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Website>)response).Item.Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Website()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();

            // Act
            var website = new Website
            {
                Name = "Website3",
                Culture = "cs-cz",
                IsDeleted = false
            };

            var websiteViewModel = new BaseViewModel<Website>
            {
                Item = website
            };

            var response = moqWebService.Save(websiteViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Website_By_Id()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Website_By_VanityId()
        {
            // Arrange
            var moqWebService = WebsiteSetup.SetupService();

            // Act
            var response = moqWebService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
