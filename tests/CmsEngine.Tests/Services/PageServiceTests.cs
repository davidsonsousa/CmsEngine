namespace CmsEngine.Tests.Services;

public class PageServiceTests : BaseServiceTests
{

    public PageServiceTests() : base()
    {
    }

    [Fact]
    public async Task Delete_ShouldDeletePage_WhenPageExists()
    {
        var id = Guid.NewGuid();
        var page = new Page { Title = "Test", VanityId = id };
        _pageRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(page);

        var result = await _pageService.Delete(id);

        _pageRepoMock.Verify(r => r.Delete(page), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Delete_ShouldSetError_WhenExceptionThrown()
    {
        var id = Guid.NewGuid();
        var page = new Page { Title = "Test", VanityId = id };
        _pageRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(page);
        _pageRepoMock.Setup(r => r.Delete(page)).Throws(new Exception("fail"));

        var result = await _pageService.Delete(id);

        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteRange_ShouldDeletePages()
    {
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var pages = ids.Select(id => new Page { Title = "Test", VanityId = id }).ToList();
        _pageRepoMock.Setup(r => r.GetByMultipleIdsAsync(ids)).ReturnsAsync(pages);

        var result = await _pageService.DeleteRange(ids);

        _pageRepoMock.Verify(r => r.DeleteRange(pages), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldInsertNewPage()
    {
        var editModel = new PageEditModel { /*IsNew = true,*/ Title = "NewPage" };
        _pageRepoMock.Setup(r => r.Insert(It.IsAny<Page>())).Returns(Task.CompletedTask);

        var result = await _pageService.Save(editModel);

        _pageRepoMock.Verify(r => r.Insert(It.IsAny<Page>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldUpdateExistingPage()
    {
        var id = Guid.NewGuid();
        var editModel = new PageEditModel { /*IsNew = false,*/ VanityId = id, Title = "ExistingPage" };
        var page = new Page { Title = "ExistingPage", VanityId = id };
        _pageRepoMock.Setup(r => r.GetForSavingById(id)).ReturnsAsync(page);

        var result = await _pageService.Save(editModel);

        _pageRepoMock.Verify(r => r.Update(It.IsAny<Page>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task GetPageCount_ShouldReturnCount()
    {
        _pageRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(3);

        var count = await _pageService.GetPageCount();

        Assert.Equal(3, count);
    }

    [Fact]
    public void SetupEditModel_ShouldReturnNewEditModel()
    {
        var result = _pageService.SetupEditModel();

        Assert.NotNull(result);
        Assert.IsType<PageEditModel>(result);
    }
}