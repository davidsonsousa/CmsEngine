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
    public class TagServiceTest : IClassFixture<TagFixture>
    {
        private TagFixture tagFixture;
        private TagService moqTagService;

        public TagServiceTest(TagFixture fixture)
        {
            tagFixture = fixture;
            moqTagService = tagFixture.Service;
        }

        #region Get

        [Fact]
        public void GetAll_ShouldReturnAllTagsAsQueryable()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().Count;

            // Act
            var response = moqTagService.GetAll();

            // Assert
            Assert.True(response is IQueryable<Tag>, "Response is not IQueryable<Tag>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetAllReadOnly_ShouldReturnAllTagsAsEnumerable()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().Count;

            // Act
            var response = moqTagService.GetAllReadOnly();

            // Assert
            Assert.True(response is IEnumerable<Tag>, "Response is not IEnumerable<Tag>");
            Assert.Equal(expectedResult, response.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqTagService.GetById(1);

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        [Fact]
        public void GetByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqTagService.GetById(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.Equal(expectedResult, response.Name);
        }

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewTag()
        {
            // Arrange

            // Act
            var response = moqTagService.SetupEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(((TagEditModel)response).IsNew, "Item is not new");
        }

        [Fact]
        public void SetupEditModel_ById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqTagService.SetupEditModel(2);

            // Assert
            Assert.IsType(typeof(TagEditModel), response);
            Assert.Equal(expectedResult, ((TagEditModel)response).Name);
        }

        [Fact]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqTagService.SetupEditModel(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.IsType(typeof(TagEditModel), response);
            Assert.Equal(expectedResult, ((TagEditModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqTagService.SetupViewModel(2);

            // Assert
            Assert.IsType(typeof(TagViewModel), response);
            Assert.Equal(expectedResult, ((TagViewModel)response).Name);
        }

        [Fact]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = tagFixture.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89")).Name;

            // Act
            var response = moqTagService.SetupViewModel(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.IsType(typeof(TagViewModel), response);
            Assert.Equal(expectedResult, ((TagViewModel)response).Name);
        }

        #endregion

        #region DB Changes

        [Fact]
        public void Save_Tag()
        {
            // Arrange

            // Act
            var tagEditModel = new TagEditModel
            {
                Name = "Tag3"
            };

            var response = moqTagService.Save(tagEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Tag_By_Id()
        {
            // Arrange

            // Act
            var response = moqTagService.Delete(1);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        [Fact]
        public void Delete_Tag_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqTagService.Delete(new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"));

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
