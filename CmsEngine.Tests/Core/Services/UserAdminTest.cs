using CmsEngine.Data.EditModels;
using Xunit;

namespace CmsEngine.Tests.Core.Services
{
    public class UserServiceTest : IClassFixture<TestFixture>
    {
        private TestFixture testFixture;
        private CmsService moqUserService;

        public UserServiceTest(TestFixture fixture)
        {
            testFixture = fixture;
            moqUserService = testFixture.Service;
        }

        #region Get

        //[Fact]
        //public void GetAll_ShouldReturnAllUsersAsQueryable()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().Count;

        //    // Act
        //    var response = moqUserService.GetAll();

        //    // Assert
        //    Assert.True(response is IQueryable<User>, "Response is not IQueryable<User>");
        //    Assert.Equal(expectedResult, response.Count());
        //}

        //[Fact]
        //public void GetAllReadOnly_ShouldReturnAllUsersAsEnumerable()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().Count;

        //    // Act
        //    var response = (IEnumerable<UserViewModel>)moqUserService.GetAllUsersReadOnly();

        //    // Assert
        //    Assert.True(response is IEnumerable<UserViewModel>, "Response is not IEnumerable<UserViewModel>");
        //    Assert.Equal(expectedResult, response.Count());
        //}

        //[Fact]
        //public void GetById_ShouldReturnCorrectUser()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().FirstOrDefault(q => q.Id == "8633a850-128f-4425-a2ec-30e23826b7ff").Name;

        //    // Act
        //    var response = (UserViewModel)moqUserService.GetUserById(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

        //    // Assert
        //    Assert.Equal(expectedResult, response.Name);
        //}

        //[Fact]
        //public void GetByUsername_ShouldReturnCorrectUser()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().FirstOrDefault(q => q.Id == "8633a850-128f-4425-a2ec-30e23826b7ff").Name;

        //    // Act
        //    var response = (UserViewModel)moqUserService.GetUserByUsername("janedeo@something.com");

        //    // Assert
        //    Assert.Equal(expectedResult, response.Name);
        //}

        #endregion

        #region Setup

        [Fact]
        public void SetupEditModel_ShouldReturnNewUser()
        {
            // Arrange

            // Act
            var response = (UserEditModel)moqUserService.SetupUserEditModel();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsNew, "Item is not new");
        }

        //[Fact]
        //public void SetupEditModel_ById_ShouldReturnCorrectUser()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().FirstOrDefault(q => q.Id == "8633a850-128f-4425-a2ec-30e23826b7ff").Name;

        //    // Act
        //    var response = (UserEditModel)moqUserService.SetupUserEditModel(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

        //    // Assert
        //    Assert.IsType<UserEditModel>(response);
        //    Assert.Equal(expectedResult, response.Name);
        //}

        //[Fact]
        //public void SetupEditModel_ByVanityId_ShouldReturnCorrectUser()
        //{
        //    // Arrange
        //    var expectedResult = testFixture.GetTestUsers().FirstOrDefault(q => q.UserName == "janedeo@something.com").Name;

        //    // Act
        //    var response = (UserEditModel)moqUserService.SetupUserEditModel("janedeo@something.com");

        //    // Assert
        //    Assert.IsType<UserEditModel>(response);
        //    Assert.Equal(expectedResult, response.Name);
        //}

        #endregion

        #region DB Changes

        [Fact]
        public void Save_User()
        {
            // Arrange

            // Act
            var userEditModel = new UserEditModel
            {
                Name = "User3"
            };

            var response = moqUserService.SaveUser(userEditModel);

            // Assert
            Assert.False(response.IsError, "Exception thrown");
        }

        //[Fact]
        //public void Delete_User_By_Id()
        //{
        //    // Arrange

        //    // Act
        //    var response = moqUserService.DeleteUser(new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"));

        //    // Assert
        //    Assert.False(response.IsError, "Exception thrown");
        //}

        //[Fact]
        //public void Delete_User_By_UserName()
        //{
        //    // Arrange

        //    // Act
        //    var response = moqUserService.DeleteUser("johndoe@mail.com");

        //    // Assert
        //    Assert.False(response.IsError, "Exception thrown");
        //}

        #endregion
    }
}
