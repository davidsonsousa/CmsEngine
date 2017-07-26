using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using CmsEngine.Tests.Core.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CmsEngine.Tests.Core.Services
{
    public class WebsiteServiceTest : IClassFixture<WebsiteFixture>
    {
        private WebsiteFixture websiteFixture;
        private WebsiteService moqWebsiteService;

        public WebsiteServiceTest(WebsiteFixture fixture)
        {
            websiteFixture = fixture;
            moqWebsiteService = websiteFixture.Service;
        }

        #region Get

        [Fact]
        public void GetAll_ShouldReturnAllWebsitesAsQueryable()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().Count;

            // Act
            var response = moqWebsiteService.GetAll();

            // Assert
            Assert.True(response is IQueryable<Website>, "Response is not IQueryable<Website>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllWebsitesAsEnumerable()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().Count;

            // Act
            var response = moqWebsiteService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<Website>, "Response is not IEnumerable<Website>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqWebsiteService.GetById(1);

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewWebsite()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(((WebsiteEditModel)response).IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebsiteService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(WebsiteEditModel), response);
            Assert.Equal(expectedResult, ((WebsiteEditModel)response).Name);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(WebsiteEditModel), response);
            Assert.Equal(expectedResult, ((WebsiteEditModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ById_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebsiteService.SetupViewModel(2);

            // Assert
            Assert.IsType(typeof(WebsiteViewModel), response);
            Assert.Equal(expectedResult, ((WebsiteViewModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectWebsite()
        {
            // Arrange
            var expectedResult = websiteFixture.GetTestWebsites().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebsiteService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsType(typeof(WebsiteViewModel), response);
            Assert.Equal(expectedResult, ((WebsiteViewModel)response).Name);
        }

        #endregion

        #region DB Changes

        [Fact]
        public void Save_Website()
        {
            // Arrange

            // Act
            var websiteEditModel = new WebsiteEditModel
            {
                Name = "Website3",
                Culture = "cs-cz"
            };

            var response = moqWebsiteService.Save(websiteEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Website_By_Id()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.Delete(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Website_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqWebsiteService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
