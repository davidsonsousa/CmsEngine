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
    public class TagServiceTest
    {
        private TagService moqTagService;

        [TestInitialize]
        public void InitializeTest()
        {
            moqTagService = TagSetup.SetupService();
        }

        #region Get

        [TestMethod]
        public void GetAll_ShouldReturnAllTagsAsQueryable()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().Count;

            // Act
            var response = moqTagService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Tag>, "Response is not IQueryable<Tag>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetAllReadOnly_ShouldReturnAllTagsAsEnumerable()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().Count;

            // Act
            var response = moqTagService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Tag>, "Response is not IEnumerable<Tag>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqTagService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void GetByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqTagService.GetById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void SetupEditModel_ShouldReturnNewTag()
        {
            // Arrange

            // Act
            var response = moqTagService.SetupEditModel();

            // Assert
            Assert.IsNotNull(response, "Item doesn't exist");
            Assert.IsTrue(((TagEditModel)response).IsNew, "Item is not new");
        }

        [TestMethod]
        public void SetupEditModel_ById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqTagService.SetupEditModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(TagEditModel));
            Assert.AreEqual(((TagEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupEditModel_ByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqTagService.SetupEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(TagEditModel));
            Assert.AreEqual(((TagEditModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ById_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqTagService.SetupViewModel(2);

            // Assert
            Assert.IsInstanceOfType(response, typeof(TagViewModel));
            Assert.AreEqual(((TagViewModel)response).Name, expectedResult);
        }

        [TestMethod]
        public void SetupViewModel_ByVanityId_ShouldReturnCorrectTag()
        {
            // Arrange
            var expectedResult = TagSetup.GetTestTags().FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqTagService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsInstanceOfType(response, typeof(TagViewModel));
            Assert.AreEqual(((TagViewModel)response).Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
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
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Tag_By_Id()
        {
            // Arrange

            // Act
            var response = moqTagService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Tag_By_VanityId()
        {
            // Arrange

            // Act
            var response = moqTagService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion
    }
}
