using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CmsEngine.Tests.Fixtures;

namespace CmsEngine.Test.Core.Services
{
    public class CategoryServiceTest : IClassFixture<CategoryFixture>
    {
        private CategoryFixture categoryFixture;
        private CategoryService moqCategoryService;

        public CategoryServiceTest(CategoryFixture fixture)
        {
            categoryFixture = fixture;
            moqCategoryService = categoryFixture.Service;
        }

        #region Get

        [Fact]
        public void GetAll_ShouldReturnAllCategoriesAsQueryable()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().Count;

            // Act
            var response = moqCategoryService.GetAll();

            // Assert
            Assert.True(response is IQueryable<Category>, "Response is not IQueryable<Category>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllCategoriesAsEnumerable()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().Count;

            // Act
            var response = moqCategoryService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<Category>, "Response is not IEnumerable<Category>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqCategoryService.GetById(1);

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqCategoryService.GetById(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewCategory()
        {
            // Arrange

            // Act
            var response = moqCategoryService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(((CategoryEditModel)response).IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqCategoryService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(CategoryEditModel), response);
            Assert.Equal(expectedResult, ((CategoryEditModel)response).Name);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqCategoryService.SetupEditModel(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.IsType(typeof(CategoryEditModel), response);
            Assert.Equal(expectedResult, ((CategoryEditModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(2);

            // Assert
            Assert.IsType(typeof(CategoryViewModel), response);
            Assert.Equal(expectedResult, ((CategoryViewModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqCategoryService.SetupViewModel(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.IsType(typeof(CategoryViewModel), response);
            Assert.Equal(expectedResult, ((CategoryViewModel)response).Name);
        }

        #endregion

        #region DB Changes

        [Fact]
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
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Category_By_Id()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Category_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqCategoryService.Delete(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
