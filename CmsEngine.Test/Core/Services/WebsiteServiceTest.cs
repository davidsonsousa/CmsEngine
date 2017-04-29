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
    public class WebsiteServiceTest
    {
        #region Get

        [TestMethod]
        public void Get_All_Websites_Queryable()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.Count;

            // Act
            var response = moqWebsiteService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Website>, "Response is not IQueryable<Website>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_All_Websites_ReadOnly()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.Count;

            // Act
            var response = moqWebsiteService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Website>, "Response is not IEnumerable<Website>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_Website_By_Id()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqWebsiteService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void Get_Website_By_VanityId()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void Setup_Website_ViewModel_Return_New_Item()
        {
            // Arrange
            var moqWebService = this.SetupWebsiteService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Website>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Website>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void Setup_Website_ViewModel_Return_Multiple_Items()
        {
            // Arrange
            var moqWebService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Website>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void Setup_Website_ViewModel_Get_Item_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Website>)response).Item.Name, expectedResult);
        }

        [TestMethod]
        public void Setup_Website_ViewModel_Get_Item_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupWebsiteService();
            var expectedResult = ListOfWebsites.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

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
            var moqWebService = this.SetupWebsiteService();

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
            var moqWebService = this.SetupWebsiteService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Website_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupWebsiteService();

            // Act
            var response = moqWebService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion

        #region Test configuration

        /// <summary>
        /// Returns a list of websites
        /// </summary>
        public List<Website> ListOfWebsites
        {
            get
            {
                return new List<Website>
                {
                    new Website { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Website1", Culture="en-US", Description="Welcome to website 1", IsDeleted = false },
                    new Website { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture="pt-BR", Description="Welcome to website 2", IsDeleted = false }
                };
            }
        }

        private WebsiteService SetupWebsiteService()
        {
            // Setup the values the repository should return
            var moqRepository = new Mock<IRepository<Website>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Website, bool>>>())).Returns(ListOfWebsites.AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Website, bool>>>())).Returns(ListOfWebsites);

            // Setup our unit of work
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(moqRepository.Object);

            return new WebsiteService(moqUnitOfWork.Object);
        }

        #endregion
    }
}
