namespace CmsEngine.Tests.Application.Services;

public class ServiceTests : BaseServiceTests
{
    public ServiceTests() : base()
    {
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenUnitOfWorkIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Service(null, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object));
    }

    [Fact]
    public void ValidatePage_ShouldReturnAtLeastOne()
    {
        var service = CreateService();
        Assert.Equal(1, service.GetType().GetMethod("ValidatePage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(service, new object[] { 0 }));
        Assert.Equal(5, service.GetType().GetMethod("ValidatePage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(service, new object[] { 5 }));
    }

    [Fact]
    public void SaveInstanceToCache_ShouldSetCache()
    {
        var service = CreateService();
        var instance = new object();
        _cacheServiceMock.Setup(m => m.Set(It.IsAny<string>(), instance, It.IsAny<TimeSpan>()));

        service.GetType().GetMethod("SaveInstanceToCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(service, new object[] { instance });

        _cacheServiceMock.Verify(m => m.Set(It.IsAny<string>(), instance, It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnUser()
    {
        var service = CreateService();
        var appUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), Name = "Test", Surname = "User", Email = "test@example.com", UserName = "testuser" };
        _userManagerMock.Setup(u => u.FindByNameAsync("testuser")).ReturnsAsync(appUser);

        var method = service.GetType().GetMethod("GetCurrentUserAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task<ApplicationUser>)method.Invoke(service, null);
        var result = await task;

        Assert.Equal("Test", result.Name);
        Assert.Equal("testuser", result.UserName);
    }

    [Fact]
    public void Dispose_ShouldCallUnitOfWorkDispose()
    {
        var service = CreateService();
        service.Dispose();
        _uowMock.Verify(u => u.Dispose(), Times.Once);
    }

    private Service CreateService()
    {
        return new Service(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object);
    }
}