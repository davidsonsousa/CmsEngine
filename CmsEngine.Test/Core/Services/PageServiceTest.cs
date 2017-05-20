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
    public class PageServiceTest
    {
        private PageService moqPageService;

        [TestInitialize]
        public void InitializeTest()
        {
            moqPageService = PageSetup.SetupService();
        }

        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllPagesAsQueryable()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().Count;

            // Act
            var response = moqPageService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Page>, "Response is not IQueryable<Page>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetAllReadOnly_ShouldReturnAllPagesAsEnumerable()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().Count;

            // Act
            var response = moqPageService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Page>, "Response is not IEnumerable<Page>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPageService.GetById(1);

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        [TestMethod]
        public void GetByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupEditModel_ShouldReturnNewPage()
        {
            // Arrange

            // Act
            var response = moqPageService.SetupEditModel();

            // Assert
            Assert.IsNotNull(response, "Item doesn't exist");
            Assert.IsTrue(((PageEditModel)response).IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupEditModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPageService.SetupEditModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(PageEditModel));
            Assert.AreEqual(((PageEditModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(PageEditModel));
            Assert.AreEqual(((PageEditModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPageService.SetupViewModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(PageViewModel));
            Assert.AreEqual(((PageViewModel)response).Title, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(PageViewModel));
            Assert.AreEqual(((PageViewModel)response).Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Page()
        {
            // Arrange

            // Act
            var pageEditModel = new PageEditModel
            {
                Title = "Page3"
            };

            var response = moqPageService.Save(pageEditModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Page_By_Id()
        {
            // Arrange

            // Act
            var response = moqPageService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Page_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPageService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
