using System;
using System.Collections.Generic;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using Moq;

namespace CmsEngine.Tests
{
    public sealed partial class TestFixture
    {        /// <summary>
             /// Returns a list of tags
             /// </summary>
        public List<Tag> GetTestTags()
        {
            return new List<Tag>
                {
                    new Tag { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1", IsDeleted = false },
                    new Tag { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<TagViewModel> GetTagViewModels()
        {
            return new List<TagViewModel>
                {
                    new TagViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Tag1" },
                    new TagViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public TagEditModel GetTagEditModel()
        {
            return new TagEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public TagViewModel GetTagViewModel()
        {
            return new TagViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Tag2" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupTagMapper()
        {
            _moqMapper.Setup(x => x.Map<Tag, TagEditModel>(It.IsAny<Tag>())).Returns(GetTagEditModel());
            _moqMapper.Setup(x => x.Map<Tag, TagViewModel>(It.IsAny<Tag>())).Returns(GetTagViewModel());
            _moqMapper.Setup(x => x.Map<Tag, TagViewModel>(null)).Returns<TagViewModel>(null);
            _moqMapper.Setup(x => x.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(It.IsAny<IEnumerable<Tag>>())).Returns(GetTagViewModels());
        }
    }
}
