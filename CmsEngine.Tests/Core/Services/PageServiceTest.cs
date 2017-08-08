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
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllPagesAsEnumerable()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().Count;

            // Act
            var response = (IEnumerable<PageViewModel>)moqPageService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<PageViewModel>, "Response is not IEnumerable<PageViewModel>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PageViewModel)moqPageService.GetById(2);

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PageViewModel)moqPageService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewPage()
        {
            // Arrange

            // Act
            var response = (PageEditModel)moqPageService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PageEditModel)moqPageService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(PageEditModel), response);
            Assert.Equal(expectedResult,response.Title);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = pageFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PageEditModel)moqPageService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(PageEditModel), response);
            Assert.Equal(expectedResult, response.Title);
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
