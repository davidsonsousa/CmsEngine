namespace CmsEngine.Tests.Application.Services;

public class TagServiceTests : BaseServiceTests
{
    public TagServiceTests() : base()
    {
    }

    [Fact]
    public async Task Delete_ShouldDeleteTag_WhenTagExists()
    {
        var id = Guid.NewGuid();
        var tag = new Tag { Name = "Test", VanityId = id };
        _tagRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

        var result = await _tagService.Delete(id);

        _tagRepoMock.Verify(r => r.Delete(tag), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Delete_ShouldSetError_WhenExceptionThrown()
    {
        var id = Guid.NewGuid();
        var tag = new Tag { Name = "Test", VanityId = id };
        _tagRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);
        _tagRepoMock.Setup(r => r.Delete(tag)).Throws(new Exception("fail"));

        var result = await _tagService.Delete(id);

        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteRange_ShouldDeleteTags()
    {
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var tags = ids.Select(id => new Tag { Name = "Test", VanityId = id }).ToList();
        _tagRepoMock.Setup(r => r.GetByMultipleIdsAsync(ids)).ReturnsAsync(tags);

        var result = await _tagService.DeleteRange(ids);

        _tagRepoMock.Verify(r => r.DeleteRange(tags), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldInsertNewTag()
    {
        var editModel = new TagEditModel { VanityId = Guid.Empty, Name = "NewTag" };
        _tagRepoMock.Setup(r => r.Insert(It.IsAny<Tag>())).Returns(Task.CompletedTask);

        var result = await _tagService.Save(editModel);

        _tagRepoMock.Verify(r => r.Insert(It.IsAny<Tag>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldUpdateExistingTag()
    {
        var id = Guid.NewGuid();
        var editModel = new TagEditModel { VanityId = id, Name = "ExistingTag" };
        var tag = new Tag { Name = "ExistingTag", VanityId = id };
        _tagRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

        var result = await _tagService.Save(editModel);

        _tagRepoMock.Verify(r => r.Update(It.IsAny<Tag>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task GetTagCount_ShouldReturnCount()
    {
        _tagRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(7);

        var count = await _tagService.GetTagCount();

        Assert.Equal(7, count);
    }

    [Fact]
    public void SetupEditModel_ShouldReturnNewEditModel()
    {
        var result = _tagService.SetupEditModel();

        Assert.NotNull(result);
        Assert.IsType<TagEditModel>(result);
    }
}