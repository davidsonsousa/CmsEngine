using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Tests.Fixtures;
using CmsEngine.Ui.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CmsEngine.Tests.Ui.Controllers.Api
{
    public class CategoryControllerTest : IClassFixture<CategoryFixture>
    {
        private Mock<IRepository<Category>> moqRepository;

        private CategoryFixture categoryFixture;
        private CategoryController controller;

        public CategoryControllerTest(CategoryFixture fixture)
        {
            categoryFixture = fixture;
            moqRepository = categoryFixture.MoqRepository;

            controller = new CategoryController(categoryFixture.MoqUnitOfWork.Object, categoryFixture.MoqMapper.Object, categoryFixture.MoqHttpContextAccessor.Object);
        }

        [Fact]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories();

            // Act
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as IEnumerable<CategoryViewModel>;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Count(), testResult.Count());
        }

        [Fact]
        public void GetCategoryById_ShouldReturnSelectedCategory()
        {
            // Arrange
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.Id == 2);

            // Act
            var actionResult = controller.Get(1);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as CategoryViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetCategoryById_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var actionResult = controller.Get(10);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetCategoryByVanityId_ShouldReturnSelectedCategory()
        {
            // Arrange
            var id = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff");
            var expectedResult = categoryFixture.GetTestCategories().FirstOrDefault(q => q.VanityId == id);

            // Act
            var actionResult = controller.Get(id);
            var okResult = actionResult as OkObjectResult;
            var testResult = okResult.Value as CategoryViewModel;

            // Assert
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(expectedResult.Name, testResult.Name);
        }

        [Fact]
        public void GetCategoryByVanityId_ShouldReturnNotFound()
        {
            // Arrange
            var id = new Guid("41ec584b-6d8f-4110-aef4-f9a5036b9bff");

            // Act
            var actionResult = controller.Get(id);
            var notFoundResult = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void PostCategory_ShouldReturnCreated()
        {
            // Arrange
            var categoryEditModel = new CategoryEditModel
            {
                Name = "Post Category",
                Slug = "post-category",
                Description = "Welcome to the post test category"
            };

            moqRepository.Setup(x => x.Insert(It.IsAny<Category>())).Verifiable();

            // Act
            var actionResult = controller.Post(categoryEditModel);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(createdResult);
        }

        [Fact]
        public void PostCategory_ShouldReturnBadRequest()
        {
            // Arrange
            var categoryEditModel = new CategoryEditModel
            {
                Slug = "post-category",
                Description = "Welcome to the post test category"
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Post(categoryEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void PutCategory_ShouldReturnOk()
        {
            // Arrange
            var categoryId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var categoryEditModel = new CategoryEditModel
            {
                Id = 1,
                VanityId = categoryId,
                Name = "Put Category",
                Slug = "put-category",
                Description = "Welcome to the put test category"
            };

            moqRepository.Setup(x => x.Update(It.IsAny<Category>())).Verifiable();

            // Act
            var actionResult = controller.Put(categoryId, categoryEditModel);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public void PutCategory_ShouldReturnBadRequest()
        {
            // Arrange
            var categoryId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");
            var categoryEditModel = new CategoryEditModel
            {
                Id = 1,
                VanityId = categoryId,
                Slug = "put-category",
                Description = "Welcome to the put test category"
            };

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actionResult = controller.Put(categoryId, categoryEditModel);

            // Assert
            Assert.IsType(typeof(BadRequestResult), actionResult);
        }

        [Fact]
        public void DeleteCategory_ShouldReturnOk()
        {
            // Arrange
            var categoryId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89");

            moqRepository.Setup(x => x.Update(It.IsAny<Category>())).Verifiable();

            // Act
            var actionResult = controller.Delete(categoryId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            moqRepository.Verify();
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
