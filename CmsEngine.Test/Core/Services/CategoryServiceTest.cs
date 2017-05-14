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
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c")).Name;

            // Act
            var response = moqCategoryService.GetById(new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupEditModel_ShouldReturnNewCategory()
        {
            // Arrange

            // Act
            var response = moqCategoryService.SetupEditModel();

            // Assert
            Assert.IsNotNull(response, "Item doesn't exist");
            Assert.IsTrue(((CategoryEditModel)response).IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupEditModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqCategoryService.SetupEditModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(CategoryEditModel));
            Assert.AreEqual(((CategoryEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c")).Name;

            // Act
            var response = moqCategoryService.SetupEditModel(new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(CategoryEditModel));
            Assert.AreEqual(((CategoryEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(CategoryViewModel));
            Assert.AreEqual(((CategoryViewModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = CategorySetup.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c")).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(CategoryViewModel));
            Assert.AreEqual(((CategoryViewModel)response).Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Category()
        {
            // Arrange

            // Act
            var categoryEditModel = new CategoryEditModel
            {
                Name = "Category3"
            };

            var response = moqCategoryService.Save(categoryEditModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Category_By_Id()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Category_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(new Guid("7a46d993-bde0-4c80-8b30-b06828c0680c"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
