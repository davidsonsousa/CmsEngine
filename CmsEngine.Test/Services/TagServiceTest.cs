using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.ViewModels;
using System;
using System.Linq.Expressions;

namespace CmsEngine.Test.Services
{
    [TestClass]
    public class TagServiceTest
    {
        #region Get

        [TestMethod]
        public void Get_All_Tags_Queryable()
        {
            // Arrange
            var moqTagService = this.SetupTagService();
            var expectedResult = ListOfTags.Count;

            // Act
            var response = moqTagService.GetAll();

            // Assert
            Assert.IsTrue(response is IQueryable<Tag>, "Response is not IQueryable<Tag>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_All_Tags_ReadOnly()
        {
            // Arrange
            var moqTagService = this.SetupTagService();
            var expectedResult = ListOfTags.Count;

            // Act
            var response = moqTagService.GetAllReadOnly();

            // Assert
            Assert.IsTrue(response is IEnumerable<Tag>, "Response is not IEnumerable<Tag>");
            Assert.AreEqual(response.Count(), expectedResult, "The number of return items do not match the expected");
        }

        [TestMethod]
        public void Get_Tag_By_Id()
        {
            // Arrange
            var moqTagService = this.SetupTagService();
            var expectedResult = ListOfTags.FirstOrDefault(q => q.Id == 1).Name;

            // Act
            var response = moqTagService.GetById(1);

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        [TestMethod]
        public void Get_Tag_By_VanityId()
        {
            // Arrange
            var moqTagService = this.SetupTagService();
            var expectedResult = ListOfTags.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqTagService.GetByVanityId(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(response.Name, expectedResult);
        }

        #endregion

        #region Setup

        [TestMethod]
        public void Setup_Tag_ViewModel_Return_New_Item()
        {
            // Arrange
            var moqWebService = this.SetupTagService();

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreNotEqual(((BaseViewModel<Tag>)response).Item, null, "Item doesn't exist");
            Assert.IsTrue(((BaseViewModel<Tag>)response).Item.IsNew, "Item is not new");
        }

        [TestMethod]
        public void Setup_Tag_ViewModel_Return_Multiple_Items()
        {
            // Arrange
            var moqWebService = this.SetupTagService();
            var expectedResult = ListOfTags.Count;

            // Act
            var response = moqWebService.SetupViewModel();

            // Assert
            Assert.AreEqual(((BaseViewModel<Tag>)response).Items.Count(), expectedResult);
        }

        [TestMethod]
        public void Setup_Tag_ViewModel_Get_Item_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupTagService();
            var expectedResult = ListOfTags.FirstOrDefault(q => q.Id == 2).Name;

            // Act
            var response = moqWebService.SetupViewModel(2);

            // Assert
            Assert.AreEqual(((BaseViewModel<Tag>)response).Item.Name, expectedResult);
        }

        [TestMethod]
        public void Setup_Tag_ViewModel_Get_Item_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupTagService();
            var expectedResult = ListOfTags.FirstOrDefault(q => q.VanityId == new Guid("8633a850-128f-4425-a2ec-30e23826b7ff")).Name;

            // Act
            var response = moqWebService.SetupViewModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.AreEqual(((BaseViewModel<Tag>)response).Item.Name, expectedResult);
        }

        #endregion

        #region DB Changes

        [TestMethod]
        public void Save_Tag()
        {
            // Arrange
            var moqWebService = this.SetupTagService();

            // Act
            var tag = new Tag
            {
                Name = "Tag3",
                IsDeleted = false
            };

            var tagViewModel = new BaseViewModel<Tag>
            {
                Item = tag
            };

            var response = moqWebService.Save(tagViewModel);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Tag_By_Id()
        {
            // Arrange
            var moqWebService = this.SetupTagService();

            // Act
            var response = moqWebService.Delete(1);

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        [TestMethod]
        public void Delete_Tag_By_VanityId()
        {
            // Arrange
            var moqWebService = this.SetupTagService();

            // Act
            var response = moqWebService.Delete(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

            // Assert
            Assert.IsFalse(response.IsError, "Exception thrown");
        }

        #endregion

        #region Test configuration

        /// <summary>
        /// Returns a list of tags
        /// </summary>
        public List<Tag> ListOfTags
        {
            get
            {
                return new List<Tag>
                {
                    new Tag { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1", IsDeleted = false },
                    new Tag { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2", IsDeleted = false }
                };
            }
        }

        private TagService SetupTagService()
        {
            // Setup the values the repository should return
            var moqRepository = new Mock<IRepository<Tag>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(ListOfTags.AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Tag, bool>>>())).Returns(ListOfTags);

            // Setup our unit of work
            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Tag>()).Returns(moqRepository.Object);

            return new TagService(moqUnitOfWork.Object);
        }

        #endregion
    }
}
