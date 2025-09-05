namespace CmsEngine.Tests.Services;

public class WebsiteServiceTests : BaseServiceTests
{

    public WebsiteServiceTests() : base()
    {
    }

    [Fact]
    public async Task Delete_ShouldDeleteWebsite_WhenWebsiteExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var website = new Website { Name = "Test", Id = 1, VanityId = id };
        _websiteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(website);

        // Act
        var result = await _websiteService.Delete(id);

        // Assert
        _websiteRepoMock.Verify(r => r.Delete(website), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Delete_ShouldSetError_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        var website = new Website { Name = "Test", Id = 1, VanityId = id };
        _websiteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(website);
        _websiteRepoMock.Setup(r => r.Delete(website)).Throws(new Exception("fail"));

        // Act
        var result = await _websiteService.Delete(id);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteRange_ShouldDeleteWebsites()
    {
        // Arrange
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var websites = ids.Select(id => new Website { Name = "Test", VanityId = id }).ToList();
        _websiteRepoMock.Setup(r => r.GetByMultipleIdsAsync(ids)).ReturnsAsync(websites);

        // Act
        var result = await _websiteService.DeleteRange(ids);

        // Assert
        _websiteRepoMock.Verify(r => r.DeleteRange(websites), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldInsertNewWebsite()
    {
        // Arrange
        var editModel = new WebsiteEditModel { /*IsNew = true,*/ Name = "NewSite" };
        _websiteRepoMock.Setup(r => r.Insert(It.IsAny<Website>())).Returns(Task.CompletedTask);

        // Act
        var result = await _websiteService.Save(editModel);

        // Assert
        _websiteRepoMock.Verify(r => r.Insert(It.IsAny<Website>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldUpdateExistingWebsite()
    {
        // Arrange
        var id = Guid.NewGuid();
        var editModel = new WebsiteEditModel { /*IsNew = false,*/ VanityId = id, Name = "ExistingSite" };
        var website = new Website { Name = "ExistingSite", VanityId = id };
        _websiteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(website);

        // Act
        var result = await _websiteService.Save(editModel);

        // Assert
        _websiteRepoMock.Verify(r => r.Update(It.IsAny<Website>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public void SetupEditModel_ShouldReturnNewEditModel()
    {
        // Act
        var result = _websiteService.SetupEditModel();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WebsiteEditModel>(result);
    }
}