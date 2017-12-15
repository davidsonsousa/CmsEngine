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
        /// Returns a list of pages
        /// </summary>
        public List<Page> GetTestPages()
        {
            return new List<Page>
                {
                    new Page { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Page1", IsDeleted = false },
                    new Page { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<PageViewModel> GetPageViewModels()
        {
            return new List<PageViewModel>
                {
                    new PageViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Page1" },
                    new PageViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PageEditModel GetPageEditModel()
        {
            return new PageEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PageViewModel GetPageViewModel()
        {
            return new PageViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Page2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupPageMapper()
        {
            _moqMapper.Setup(x => x.Map<Page, PageEditModel>(It.IsAny<Page>())).Returns(GetPageEditModel());
            _moqMapper.Setup(x => x.Map<Page, PageViewModel>(It.IsAny<Page>())).Returns(GetPageViewModel());
            _moqMapper.Setup(x => x.Map<Page, PageViewModel>(null)).Returns<PageViewModel>(null);
            _moqMapper.Setup(x => x.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(It.IsAny<IEnumerable<Page>>())).Returns(GetPageViewModels());
        }

    }
}
