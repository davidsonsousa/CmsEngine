namespace CmsEngine.Tests.Services;

public class XmlServiceTests : BaseServiceTests
{
    public XmlServiceTests() : base()
    {
    }

    [Fact]
    public async Task GenerateFeed_ShouldReturnRssDocument_WithPublishedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new() { Title = "Post1", Slug = "post-1", DocumentContent = "Content1", PublishedOn = DateTime.UtcNow },
            new() { Title = "Post2", Slug = "post-2", DocumentContent = "Content2", PublishedOn = DateTime.UtcNow }
        };

        _postRepoMock.Setup(r => r.GetPublishedPostsOrderByDescending(It.IsAny<Expression<Func<Post, DateTime>>>())).ReturnsAsync(posts);

        // Act
        var doc = await _xmlService.GenerateFeed();

        // Assert
        Assert.NotNull(doc);
        var channel = doc.Root?.Element("channel");
        Assert.NotNull(channel);
        Assert.Equal("TestSite", channel.Element("title")?.Value);
        Assert.Equal("Test Tagline", channel.Element("description")?.Value);
        Assert.Equal("en-us", channel.Element("language")?.Value);
        Assert.Equal(2, channel.Elements("item").Count());
    }

    [Fact]
    public async Task GenerateSitemap_ShouldReturnSitemapDocument_WithPostsAndPages()
    {
        // Arrange
        var posts = new List<Post>
        {
            new() { Slug = "post-1", PublishedOn = new DateTime(2023, 1, 1) }
        };
        var pages = new List<Page>
        {
            new() { Slug = "page-1", PublishedOn = new DateTime(2023, 2, 2) }
        };

        _postRepoMock.Setup(r => r.GetPublishedPostsOrderByDescending(It.IsAny<Expression<Func<Post, DateTime>>>())).ReturnsAsync(posts);
        _pageRepoMock.Setup(r => r.GetOrderByDescending(It.IsAny<Expression<Func<Page, DateTime>>>())).ReturnsAsync(pages);

        // Act
        var doc = await _xmlService.GenerateSitemap();

        // Assert
        Assert.NotNull(doc);
        var urlset = doc.Root;
        Assert.NotNull(urlset);
        Assert.Equal("urlset", urlset.Name.LocalName);
        Assert.Equal(2, urlset.Elements().Count());
    }

    [Theory]
    [InlineData("", "", "https://test.com")]
    [InlineData("", "about-me", "https://test.com/about-me")]
    [InlineData("blog/post/", "random-article", "https://test.com/blog/post/random-article")]
    public void FormatUrl_ShouldFormatCorrectly(string type, string slug, string expected)
    {
        // Act
        var url = typeof(XmlService).GetMethod("FormatUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                    .Invoke(_xmlService, new object[] { type, slug }) as string;

        // Assert
        Assert.Equal(expected, url);
    }
}