using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.ViewModels;
using Xunit;

namespace CmsEngine.Tests.Core.Services
{
    public class CategoryServiceTest : IClassFixture<TestFixture>
    {
        private TestFixture testFixture;
        private CmsService moqCategoryService;

        public CategoryServiceTest(TestFixture fixture)
        {
            testFixture = fixture;
            moqCategoryService = testFixture.Service;
        }

        #region Get

        //[Fact]
        //public void GetAll_ShouldReturnAllCategoriesAsQueryable()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestCategories().Count;

        //    // Act
        //    var response = moqCategoryService.GetAll();

        //    // Assert
        //    Assert.True(response is IQueryable<Category>, "Response is not IQueryable<Category>");
        //    Assert.Equal(expectedResult, response.Count());
        //}

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllCategoriesAsEnumerable()
        {
            // Arrange
            var expectedResult = testFixture.GetTestCategories().Count;

            // Act
            var response = (IEnumerable<CategoryViewModel>)moqCategoryService.GetAllCategoriesReadOnly();

            // Assert
            Assert.True(response is IEnumerable<CategoryViewModel>, "Response is not IEnumerable<CategoryViewModel>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = testFixture.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = (CategoryViewModel)moqCategoryService.GetCategoryById(2);

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = testFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = (CategoryViewModel)moqCategoryService.GetCategoryById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

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
            var response = (CategoryEditModel)moqCategoryService.SetupCategoryEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = testFixture.GetTestCategories().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = (CategoryEditModel)moqCategoryService.SetupCategoryEditModel(2);

            // Assert
            Assert.IsType<CategoryEditModel>(response);
            Assert.Equal(expectedResult, response.Name);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedResult = testFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = (CategoryEditModel)moqCategoryService.SetupCategoryEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType<CategoryEditModel>(response);
            Assert.Equal(expectedResult, response.Name);
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

            var response = moqCategoryService.SaveCategory(categoryEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Category_By_Id()
        {
            // Arrange

            // Act
            var response = moqCategoryService.DeleteCategory(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Category_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqCategoryService.DeleteCategory(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
