using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.ViewModels;
using Xunit;

namespace CmsEngine.Tests.Core.Services
{
    public class PageServiceTest : IClassFixture<TestFixture>
    {
        private TestFixture testFixture;
        private CmsService moqPageService;

        public PageServiceTest(TestFixture fixture)
        {
            testFixture = fixture;
            moqPageService = testFixture.Service;
        }

        #region Get

        //[Fact]
        //public void GetAll_ShouldReturnAllPagesAsQueryable()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestPages().Count;

        //    // Act
        //    var response = moqPageService.GetAll();

        //    // Assert
        //    Assert.True(response is IQueryable<Page>, "Response is not IQueryable<Page>");
        //    Assert.Equal(expectedResult, response.Count());
        //}

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllPagesAsEnumerable()
        {
            // Arrange
            var expectedResult = testFixture.GetTestPages().Count;

            // Act
            var response = (IEnumerable<PageViewModel>)moqPageService.GetAllPagesReadOnly();

            // Assert
            Assert.True(response is IEnumerable<PageViewModel>, "Response is not IEnumerable<PageViewModel>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = testFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PageViewModel)moqPageService.GetPageById(2);

            // Assert
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = testFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PageViewModel)moqPageService.GetPageById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

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
            var response = (PageEditModel)moqPageService.SetupPageEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = testFixture.GetTestPages().FirstOrDefault(q => q.Id == 2).Title;

            // Act
            var response = (PageEditModel)moqPageService.SetupPageEditModel(2);

            // Assert
            Assert.IsType<PageEditModel>(response);
            Assert.Equal(expectedResult, response.Title);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectPage()
        {
            // Arrange
            var expectedResult = testFixture.GetTestPages().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Title;

            // Act
            var response = (PageEditModel)moqPageService.SetupPageEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType<PageEditModel>(response);
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

            var response = moqPageService.SavePage(pageEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Page_By_Id()
        {
            // Arrange

            // Act
            var response = moqPageService.DeletePage(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Page_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqPageService.DeletePage(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
