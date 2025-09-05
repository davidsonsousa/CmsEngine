namespace CmsEngine.Tests.Services;

public class CategoryServiceTests : BaseServiceTests
{

    public CategoryServiceTests() : base()
    {
    }

    [Fact]
    public async Task Delete_ShouldDeleteCategory_WhenCategoryExists()
    {
        var id = Guid.NewGuid();
        var category = new Category { Name = "Test", VanityId = id };
        _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);

        var result = await _categoryService.Delete(id);

        _categoryRepoMock.Verify(r => r.Delete(category), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Delete_ShouldSetError_WhenExceptionThrown()
    {
        var id = Guid.NewGuid();
        var category = new Category { Name = "Test", VanityId = id };
        _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);
        _categoryRepoMock.Setup(r => r.Delete(category)).Throws(new Exception("fail"));

        var result = await _categoryService.Delete(id);

        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteRange_ShouldDeleteCategories()
    {
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var categories = ids.Select(id => new Category { Name = "Test", VanityId = id }).ToList();
        _categoryRepoMock.Setup(r => r.GetByMultipleIdsAsync(ids)).ReturnsAsync(categories);

        var result = await _categoryService.DeleteRange(ids);

        _categoryRepoMock.Verify(r => r.DeleteRange(categories), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("deleted", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldInsertNewCategory()
    {
        var editModel = new CategoryEditModel { /*IsNew = true,*/ Name = "NewCat" };
        _categoryRepoMock.Setup(r => r.Insert(It.IsAny<Category>())).Returns(Task.CompletedTask);

        var result = await _categoryService.Save(editModel);

        _categoryRepoMock.Verify(r => r.Insert(It.IsAny<Category>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldUpdateExistingCategory()
    {
        var id = Guid.NewGuid();
        var editModel = new CategoryEditModel { /*IsNew = false,*/ VanityId = id, Name = "ExistingCat" };
        var category = new Category { Name = "ExistingCat", VanityId = id };
        _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);

        var result = await _categoryService.Save(editModel);

        _categoryRepoMock.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task GetCategoryCount_ShouldReturnCount()
    {
        _categoryRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(5);

        var count = await _categoryService.GetCategoryCount();

        Assert.Equal(5, count);
    }

    [Fact]
    public void SetupEditModel_ShouldReturnNewEditModel()
    {
        var result = _categoryService.SetupEditModel();

        Assert.NotNull(result);
        Assert.IsType<CategoryEditModel>(result);
    }
}