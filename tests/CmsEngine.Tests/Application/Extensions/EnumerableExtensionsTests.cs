namespace CmsEngine.Tests.Application.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void PopulateSelectList_ReturnsCorrectSelectListItems()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var items = new List<TestViewModel>
        {
            new TestViewModel { Name = "Alpha", VanityId = id1 },
            new TestViewModel { Name = "Beta", VanityId = id2 }
        };

        var selected = new List<string> { id2.ToString() };
        var result = items.PopulateSelectList(selected).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Alpha", result[0].Text);
        Assert.Equal(id1.ToString(), result[0].Value);
        Assert.False(result[0].Selected);
        Assert.Equal("Beta", result[1].Text);
        Assert.True(result[1].Selected);
    }

    [Fact]
    public void PopulateCheckboxList_ReturnsCorrectCheckboxEditModels()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var items = new List<TestViewModel>
        {
            new TestViewModel { Name = "Alpha", VanityId = id1 },
            new TestViewModel { Name = "Beta", VanityId = id2 }
        };

        var selected = new List<string> { id1.ToString() };
        var result = items.PopulateCheckboxList(selected).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Alpha", result[0].Label);
        Assert.Equal(id1.ToString(), result[0].Value);
        Assert.True(result[0].Selected);
        Assert.True(result[0].Enabled);
        Assert.Equal("Beta", result[1].Label);
        Assert.False(result[1].Selected);
    }

    [Fact]
    public void GetSearchExpression_ReturnsExpression()
    {
        var items = new List<TestViewModel>
        {
            new TestViewModel { Name = "Alpha", VanityId = Guid.NewGuid() },
            new TestViewModel { Name = "Beta", VanityId = Guid.NewGuid() }
        };

        var properties = typeof(TestViewModel).GetProperties().Where(p => p.Name == "Name");
        var expr = items.GetSearchExpression("Alpha", properties);

        Assert.NotNull(expr);
        // Compile and test the expression
        var func = expr.Compile();
        Assert.True(func(items[0]));
        Assert.False(func(items[1]));
    }
}
