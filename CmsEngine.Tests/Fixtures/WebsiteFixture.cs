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
        /// Returns a list of websites
        /// </summary>
        public List<Website> GetTestWebsites()
        {
            return new List<Website>
                {
                    new Website { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Website1", Culture="en-US", Description="Welcome to website 1", IsDeleted = false },
                    new Website { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture="pt-BR", Description="Welcome to website 2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<WebsiteViewModel> GetWebsiteViewModels()
        {
            return new List<WebsiteViewModel>
                {
                    new WebsiteViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Website1", Culture="en-US", Description="Welcome to website 1" },
                    new WebsiteViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture="pt-BR", Description="Welcome to website 2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public WebsiteEditModel GetWebsiteEditModel()
        {
            return new WebsiteEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture = "pt-BR", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public WebsiteViewModel GetWebsiteViewModel()
        {
            return new WebsiteViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture = "pt-BR", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupWebsiteMapper()
        {
            _moqMapper.Setup(x => x.Map<Website, WebsiteEditModel>(It.IsAny<Website>())).Returns(GetWebsiteEditModel());
            _moqMapper.Setup(x => x.Map<Website, WebsiteViewModel>(It.IsAny<Website>())).Returns(GetWebsiteViewModel());
            _moqMapper.Setup(x => x.Map<Website, WebsiteViewModel>(null)).Returns<WebsiteViewModel>(null);
            _moqMapper.Setup(x => x.Map<IEnumerable<Website>, IEnumerable<WebsiteViewModel>>(It.IsAny<IEnumerable<Website>>())).Returns(GetWebsiteViewModels());
        }
    }
}
