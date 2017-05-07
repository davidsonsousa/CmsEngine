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
    public class CategoryServiceTest
    {
        private CategoryService moqCategoryService;

        [TestInitialize]
        public void InitializeTest()
        {
            moqCategoryService = CategorySetup.SetupService();
        }

        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllCategoriesAsQueryable()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().Count;

            // Act
            var response = moqCategoryService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Category>, "Response is not IQueryable<Category>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetAllReadOnly_ShouldReturnAllCategoriesAsEnumerable()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().Count;

            // Act
            var response = moqCategoryService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Category>, "Response is not IEnumerable<Category>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqCategoryService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void GetByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8")).Name;

            // Act
            var response = moqCategoryService.GetByVanityId(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupViewModel_ShouldReturnNewCategory()
        {
            // Arrange

            // Act
            var response = moqCategoryService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Category>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Category>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupViewModel_ShouldReturnAllCategories()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().Count;

            // Act
            var response = moqCategoryService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Item.Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8")).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Category>)response).Item.Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save()
        {
            // Arrange

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

            var response = moqCategoryService.Save(categoryViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_By_Id()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(new Guid("aa2a8fea-cace-482f-b8f5-58a41b9a03f8"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
