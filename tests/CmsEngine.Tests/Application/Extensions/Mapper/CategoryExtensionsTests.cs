namespace CmsEngine.Tests.Application.Extensions.Mapper;

public class CategoryExtensionsTests
{
    [Fact]
    public void MapToEditModel_MapsCategoryToEditModel()
    {
        var category = new Category
        {
            Id = 1,
            VanityId = Guid.NewGuid(),
            Name = "Test",
            Description = "Desc",
            Slug = "test-slug"
        };

        var result = category.MapToEditModel();

        Assert.Equal(category.Id, result.Id);
        Assert.Equal(category.VanityId, result.VanityId);
        Assert.Equal(category.Name, result.Name);
        Assert.Equal(category.Description, result.Description);
        Assert.Equal(category.Slug, result.Slug);
    }

    [Fact]
    public void MapToModel_MapsEditModelToCategory()
    {
        var editModel = new CategoryEditModel
        {
            Id = 2,
            VanityId = Guid.NewGuid(),
            Name = "Edit",
            Description = "EditDesc",
            Slug = "edit-slug"
        };

        var result = editModel.MapToModel();

        Assert.Equal(editModel.Id, result.Id);
        Assert.Equal(editModel.VanityId, result.VanityId);
        Assert.Equal(editModel.Name, result.Name);
        Assert.Equal(editModel.Description, result.Description);
        Assert.Equal(editModel.Slug, result.Slug);
    }

    [Fact]
    public void MapToModel_UpdatesExistingCategory()
    {
        var editModel = new CategoryEditModel
        {
            Id = 3,
            VanityId = Guid.NewGuid(),
            Name = "Update",
            Description = "UpdateDesc",
            Slug = "update-slug"
        };
        var category = new Category();

        var result = editModel.MapToModel(category);

        Assert.Equal(editModel.Id, result.Id);
        Assert.Equal(editModel.VanityId, result.VanityId);
        Assert.Equal(editModel.Name, result.Name);
        Assert.Equal(editModel.Description, result.Description);
        Assert.Equal(editModel.Slug, result.Slug);
    }

    [Fact]
    public void MapToTableViewModel_MapsCategoriesToTableViewModels()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, VanityId = Guid.NewGuid(), Name = "A", Description = "D1", Slug = "a" },
            new Category { Id = 2, VanityId = Guid.NewGuid(), Name = "B", Description = "D2", Slug = "b" }
        };

        var result = categories.MapToTableViewModel().ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].Name);
        Assert.Equal("b", result[1].Slug);
    }

    [Fact]
    public void MapToViewModel_MapsCategoriesToViewModels()
    {
        var post = new Post { VanityId = Guid.NewGuid(), Title = "Post1", Description = "Desc", Slug = "post1", PublishedOn = DateTime.Now };
        var category = new Category
        {
            Id = 1,
            VanityId = Guid.NewGuid(),
            Name = "Cat",
            Description = "Desc",
            Slug = "cat",
            PostCategories = new List<PostCategory> { new PostCategory { Post = post } }
        };
        var categories = new List<Category> { category };

        var result = categories.MapToViewModel("yyyy-MM-dd").ToList();

        Assert.Single(result);
        Assert.Equal("Cat", result[0].Name);
        Assert.NotNull(result[0].Posts);
        Assert.Single(result[0].Posts);
        Assert.Equal("Post1", result[0].Posts.First().Title);
    }

    [Fact]
    public void MapToViewModelSimple_MapsCategoriesToSimpleViewModels()
    {
        var categories = new List<Category>
        {
            new Category { VanityId = Guid.NewGuid(), Name = "Simple", Slug = "simple" }
        };

        var result = categories.MapToViewModelSimple().ToList();

        Assert.Single(result);
        Assert.Equal("Simple", result[0].Name);
        Assert.Equal("simple", result[0].Slug);
    }

    [Fact]
    public void MapToViewModelWithPostCount_MapsCategoriesToViewModelsWithPostCount()
    {
        var categories = new List<Category>
        {
            new Category { VanityId = Guid.NewGuid(), Name = "Count", Slug = "count", PostCount = 5 }
        };

        var result = categories.MapToViewModelWithPostCount().ToList();

        Assert.Single(result);
        Assert.Equal(5, result[0].PostCount);
    }

    [Fact]
    public void MapToViewModelWithPost_MapsCategoriesToViewModelsWithPosts()
    {
        var post = new Post { VanityId = Guid.NewGuid(), Title = "Post2", Description = "Desc2", Slug = "post2", PublishedOn = DateTime.Now };
        var category = new Category
        {
            VanityId = Guid.NewGuid(),
            Name = "WithPost",
            Slug = "withpost",
            Posts = new List<Post> { post }
        };
        var categories = new List<Category> { category };

        var result = categories.MapToViewModelWithPost("yyyy-MM-dd").ToList();

        Assert.Single(result);
        Assert.Equal("WithPost", result[0].Name);
        Assert.NotNull(result[0].Posts);
        Assert.Single(result[0].Posts);
        Assert.Equal("Post2", result[0].Posts.First().Title);
    }
}
