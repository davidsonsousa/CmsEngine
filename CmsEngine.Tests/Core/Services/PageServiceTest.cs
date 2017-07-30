using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using CmsEngine.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CmsEngine.Tests.Core.Services
{
    public class PageServiceTest: IClassFixture<PageFixture>
    {
        private PageFixture pageFixture;
        private PageService moqPageService;

        public PageServiceTest(PageFixture fixture)
        {
            pageFixture = fixture;
            moqPageService = pageFixture.Service;
        }

        #region Get

        [Fact]
        public void GetAll_ShouldReturnAllPagesAsQueryable()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().Count;

            // Act
            var response = moqPageService.GetAll();

            // Assert
            Assert.True(response is IQueryable<Page>, "Response is not IQueryable<Page>");
            Assert.Equal(response.Count(), expectedResult);
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllPagesAsEnumerable()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().Count;

            // Act
            var response = moqPageService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<Page>, "Response is not IEnumerable<Page>");
            Assert.Equal(response.Count(), expectedResult);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 1).Title;

            // Act
            var response = moqPageService.GetById(1);

            // Assert
            Assert.Equal(response.Title, expectedResult);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.Equal(response.Title, expectedResult);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewPage()
        {
            // Arrange

            // Act
            var response = moqPageService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(((PageEditModel)response).IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPageService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(PageEditModel), response);
            Assert.Equal(((PageEditModel)response).Title, expectedResult);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PageEditModel), response);
            Assert.Equal(((PageEditModel)response).Title, expectedResult);
        }

        [Fact]
        public void SetupViewModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = moqPageService.SetupViewModel(2);

            // Assert
            Assert.IsType(typeof(PageViewModel), response);
            Assert.Equal(((PageViewModel)response).Title, expectedResult);
        }

        [Fact]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = moqPageService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PageViewModel), response);
            Assert.Equal(((PageViewModel)response).Title, expectedResult);
        }

        #endregion

        #region DB Changes

        [Fact]
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
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Page_By_Id()
        {
            // Arrange

            // Act
            var response = moqPageService.Delete(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Page_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPageService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
