namespace CmsEngine.Tests.Services;

public class PostServiceTests : BaseServiceTests
{

    public PostServiceTests()
    {

    }

    [Fact]
    public async Task Delete_ShouldDeletePost_WhenPostExists()
    {
        var id = Guid.NewGuid();
        var post = new Post { Title = "Test", VanityId = id };
        _postRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(post);

        var result = await _postService.Delete(id);

        _postRepoMock.Verify(r => r.Delete(post), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Delete_ShouldSetError_WhenExceptionThrown()
    {
        var id = Guid.NewGuid();
        var post = new Post { Title = "Test", VanityId = id };
        _postRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(post);
        _postRepoMock.Setup(r => r.Delete(post)).Throws(new Exception("fail"));

        var result = await _postService.Delete(id);

        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteRange_ShouldDeletePosts()
    {
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var posts = ids.Select(id => new Post { Title = "Test", VanityId = id }).ToList();
        _postRepoMock.Setup(r => r.GetByMultipleIdsAsync(ids)).ReturnsAsync(posts);

        var result = await _postService.DeleteRange(ids);

        _postRepoMock.Verify(r => r.DeleteRange(posts), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldInsertNewPost()
    {
        var editModel = new PostEditModel { VanityId = Guid.Empty, Title = "NewPost" };
        _postRepoMock.Setup(r => r.Insert(It.IsAny<Post>())).Returns(Task.CompletedTask);

        var result = await _postService.Save(editModel);

        _postRepoMock.Verify(r => r.Insert(It.IsAny<Post>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldUpdateExistingPost()
    {
        var id = Guid.NewGuid();
        var editModel = new PostEditModel { VanityId = id, Title = "ExistingPost" };
        var post = new Post { Title = "ExistingPost", VanityId = id };
        _postRepoMock.Setup(r => r.GetForSavingById(id)).ReturnsAsync(post);

        var result = await _postService.Save(editModel);

        _postRepoMock.Verify(r => r.Update(It.IsAny<Post>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task GetPostCount_ShouldReturnCount()
    {
        _postRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(4);

        var count = await _postService.GetPostCount();

        Assert.Equal(4, count);
    }

    [Fact]
    public void SetupEditModel_ShouldReturnNewEditModel()
    {
        var result = _postService.SetupEditModel();

        Assert.NotNull(result);
        Assert.IsType<PostEditModel>(result);
    }
}