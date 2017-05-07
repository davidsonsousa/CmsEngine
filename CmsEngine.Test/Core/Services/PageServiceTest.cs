using CmsEngine.Data.Models;
using CmsEngine.Services;
using CmsEngine.Test.Setup;
using CmsEngine.ViewModels;
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
            var response = moqPageService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupViewModel_ShouldReturnNewPage()
        {
            // Arrange

            // Act
            var response = moqPageService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Page>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Page>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupViewModel_ShouldReturnAllPages()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().Count;

            // Act
            var response = moqPageService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPageService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Item.Title, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = PageSetup.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Item.Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Page()
        {
            // Arrange

            // Act
            var page = new Page
            {
                Title = "Page3",
                IsDeleted = false
            };

            var pageViewModel = new BaseViewModel<Page>
            {
                Item = page
            };

            var response = moqPageService.Save(pageViewModel);

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
