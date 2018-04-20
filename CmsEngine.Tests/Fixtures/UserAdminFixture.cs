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
        /// Returns a list of users
        /// </summary>
        public List<ApplicationUser> GetTestUsers()
        {
            return new List<ApplicationUser>
                {
                    new ApplicationUser { Id = "278c0380-bdd2-45bb-869b-b94659bc2b89", Name = "John", Surname = "Doe", UserName = "johndoe@mail.com" },
                    new ApplicationUser { Id = "8633a850-128f-4425-a2ec-30e23826b7ff", Name = "Jane", Surname = "Deo", UserName = "janedeo@something.com" }
                };
        }

        /// <summary>
        /// Returns a list of ViewModels
        /// </summary>
        public List<UserViewModel> GetUserViewModels()
        {
            return new List<UserViewModel>
                {
                    new UserViewModel { VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "John", Surname = "Doe", UserName = "johndoe@mail.com" },
                    new UserViewModel { VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Jane", Surname = "Deo", UserName = "janedeo@something.com" }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public UserEditModel GetUserEditModel()
        {
            return new UserEditModel { VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Jane", Surname = "Deo", UserName = "janedeo@something.com" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public UserViewModel GetUserViewModel()
        {
            return new UserViewModel { VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Jane", Surname = "Deo", UserName = "janedeo@something.com" };
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupUserMapper()
        {
            _moqMapper.Setup(x => x.Map<ApplicationUser, UserEditModel>(It.IsAny<ApplicationUser>())).Returns(GetUserEditModel());
            _moqMapper.Setup(x => x.Map<UserEditModel, ApplicationUser>(It.IsAny<UserEditModel>())).Returns(new ApplicationUser());

            _moqMapper.Setup(x => x.Map<ApplicationUser, UserViewModel>(It.IsAny<ApplicationUser>())).Returns(GetUserViewModel());
            _moqMapper.Setup(x => x.Map<ApplicationUser, UserViewModel>(null)).Returns<UserViewModel>(null);

            _moqMapper.Setup(x => x.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(It.IsAny<IEnumerable<ApplicationUser>>())).Returns(GetUserViewModels());
        }
    }
}
