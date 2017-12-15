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
        /// Returns a list of posts
        /// </summary>
        public List<Post> GetTestPosts()
        {
            return new List<Post>
                {
                    new Post { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1", IsDeleted = false },
                    new Post { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<PostViewModel> GetPostViewModels()
        {
            return new List<PostViewModel>
                {
                    new PostViewModel { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Title = "Post1" },
                    new PostViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PostEditModel GetPostEditModel()
        {
            return new PostEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public PostViewModel GetPostViewModel()
        {
            return new PostViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Title = "Post2", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupPostMapper()
        {
            _moqMapper.Setup(x => x.Map<Post, PostEditModel>(It.IsAny<Post>())).Returns(GetPostEditModel());
            _moqMapper.Setup(x => x.Map<Post, PostViewModel>(It.IsAny<Post>())).Returns(GetPostViewModel());
            _moqMapper.Setup(x => x.Map<Post, PostViewModel>(null)).Returns<PostViewModel>(null);
            _moqMapper.Setup(x => x.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(It.IsAny<IEnumerable<Post>>())).Returns(GetPostViewModels());
        }
    }
}
