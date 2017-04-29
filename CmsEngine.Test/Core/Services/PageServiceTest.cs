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

namespace CmsEngine.Test.Core.Services
{
    [TestClass]
    public class PageServiceTest
    {
        #region Get

        [TestMethod]
        public void Get_All_Pages_Queryable()
        {
            // Arrange
            var moqPageService = this.SetupPageService();
            var expectedResult = ListOfPages.Count;

            // Act
            var response = moqPageService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Page>, "Response is not IQueryable<Page>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_All_Pages_ReadOnly()
        {
            // Arrange
            var moqPageService = this.SetupPageService();
            var expectedResult = ListOfPages.Count;

            // Act
            var response = moqPageService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Page>, "Response is not IEnumerable<Page>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_Page_By_Id()
        {
            // Arrange
            var moqPageService = this.SetupPageService();
            var expectedResult = ListOfPages.FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPageService.GetById(1);

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        [TestMethod]
        public void Get_Page_By_VanityId()
        {
            // Arrange
            var moqPageService = this.SetupPageService();
            var expectedResult = ListOfPages.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void Setup_Page_ViewModel_Return_New_Item()
        {
            // Arrange
            var moqWebService = this.SetupPageService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Page>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Page>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void Setup_Page_ViewModel_Return_Multiple_Items()
        {
            // Arrange
            var moqWebService = this.SetupPageService();
            var expectedResult = ListOfPages.Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void Setup_Page_ViewModel_Get_Item_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupPageService();
            var expectedResult = ListOfPages.FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Item.Title, expectedResult);
        }

        [TestMethod]
        public void Setup_Page_ViewModel_Get_Item_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupPageService();
            var expectedResult = ListOfPages.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqWebService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Page>)response).Item.Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Page()
        {
            // Arrange
            var moqWebService = this.SetupPageService();

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

            var response = moqWebService.Save(pageViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Page_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupPageService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Page_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupPageService();

            // Act
            var response = moqWebService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion

        #region Test configuration

        /// <summary>
        /// Returns a list of pages
        /// </summary>
        public List<Page> ListOfPages
        {
            get
            {
                return new List<Page>
                {
                    new Page { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Page1", IsDeleted = false },
                    new Page { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", IsDeleted = false }
                };
            }
        }

        private PageService SetupPageService()
        {
            // Setup the values the repository should return
            var moqRepository = new Mock<IRepository<Page>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Page, bool>>>())).Returns(ListOfPages.AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Page, bool>>>())).Returns(ListOfPages);

            // Setup our unit of work
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Page>()).Returns(moqRepository.Object);

            return new PageService(moqUnitOfWork.Object);
        }

        #endregion
    }
}
