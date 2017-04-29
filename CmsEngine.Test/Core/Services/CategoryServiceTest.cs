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
    public class CategoryServiceTest
    {
        #region Get

        [TestMethod]
        public void Get_All_Categories_Queryable()
        {
            // Arrange
            var moqCategoryService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.Count;

            // Act
            var response = moqCategoryService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Category>, "Response is not IQueryable<Category>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_All_Categories_ReadOnly()
        {
            // Arrange
            var moqCategoryService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.Count;

            // Act
            var response = moqCategoryService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Category>, "Response is not IEnumerable<Category>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_Category_By_Id()
        {
            // Arrange
            var moqCategoryService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqCategoryService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void Get_Category_By_VanityId()
        {
            // Arrange
            var moqCategoryService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.FirstOrDefault(q => q.VanityId == new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8")).Name;

            // Act
            var response = moqCategoryService.GetByVanityId(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void Setup_Category_ViewModel_Return_New_Item()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Category>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Category>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void Setup_Category_ViewModel_Return_Multiple_Items()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void Setup_Category_ViewModel_Get_Item_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Item.Name, expectedResult);
        }

        [TestMethod]
        public void Setup_Category_ViewModel_Get_Item_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();
            var expectedResult = ListOfCategories.FirstOrDefault(q => q.VanityId == new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8")).Name;

            // Act
            var response = moqWebService.SetupViewModel(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Item.Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Category()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();

            // Act
            var category = new Category
            {
                Name = "Category3",
                IsDeleted = false
            };

            var categoryViewModel = new BaseViewModel<Category>
            {
                Item = category
            };

            var response = moqWebService.Save(categoryViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Category_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Category_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupCategoryService();

            // Act
            var response = moqWebService.Delete(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion

        #region Test configuration

        /// <summary>
        /// Returns a list of categories
        /// </summary>
        public List<Category> ListOfCategories
        {
            get
            {
                return new List<Category>
                {
                    new Category { Id = 1, VanityId = new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"), Name = "Category1", IsDeleted = false },
                    new Category { Id = 2, VanityId = new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"), Name = "Category2", IsDeleted = false }
                };
            }
        }

        private CategoryService SetupCategoryService()
        {
            // Setup the values the repository should return
            var moqRepository = new Mock<IRepository<Category>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Category, bool>>>())).Returns(ListOfCategories.AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Category, bool>>>())).Returns(ListOfCategories);

            // Setup our unit of work
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Category>()).Returns(moqRepository.Object);

            return new CategoryService(moqUnitOfWork.Object);
        }

        #endregion
    }
}
