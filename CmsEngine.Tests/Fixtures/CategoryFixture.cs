using System;
using System.Collections.Generic;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using Moq;

namespace CmsEngine.Tests
{
    public sealed partial class TestFixture
    {
        /// <summary>
        /// Returns a list of categories
        /// </summary>
        public List<Category> GetTestCategories()
        {
            return new List<Category>
                {
                    new Category { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Category1", Description="Welcome to category 1", IsDeleted = false },
                    new Category { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description="Welcome to category 2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<CategoryViewModel> GetCategoryViewModels()
        {
            return new List<CategoryViewModel>
                {
                    new CategoryViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Category1", Description="Welcome to category 1" },
                    new CategoryViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description="Welcome to category 2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public CategoryEditModel GetCategoryEditModel()
        {
            return new CategoryEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description = "Welcome to category 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public CategoryViewModel GetCategoryViewModel()
        {
            return new CategoryViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Category2", Description = "Welcome to category 2" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupCategoryMapper()
        {
            _moqMapper.Setup(x => x.Map<Category, CategoryEditModel>(It.IsAny<Category>())).Returns(GetCategoryEditModel());
            _moqMapper.Setup(x => x.Map<Category, CategoryViewModel>(It.IsAny<Category>())).Returns(GetCategoryViewModel());
            _moqMapper.Setup(x => x.Map<Category, CategoryViewModel>(null)).Returns<CategoryViewModel>(null);
            _moqMapper.Setup(x => x.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(It.IsAny<IEnumerable<Category>>())).Returns(GetCategoryViewModels());
        }

    }
}
