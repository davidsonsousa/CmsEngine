namespace CmsEngine.Tests.Services;

public class BaseServiceTests
{
    protected readonly Mock<IUnitOfWork> _uowMock;
    protected readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    protected readonly Mock<ILoggerFactory> _loggerFactoryMock;
    protected readonly Mock<ILogger> _loggerMock;

    protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    protected readonly Mock<IEmailRepository> _emailRepoMock;
    protected readonly Mock<IWebsiteRepository> _websiteRepoMock;
    protected readonly Mock<ICategoryRepository> _categoryRepoMock;
    protected readonly Mock<ITagRepository> _tagRepoMock;
    protected readonly Mock<IPageRepository> _pageRepoMock;
    protected readonly Mock<IPostRepository> _postRepoMock;

    protected readonly Mock<ICacheService> _cacheServiceMock;
    protected readonly IEmailService _emailService;
    protected readonly IWebsiteService _websiteService;
    protected readonly ICategoryService _categoryService;
    protected readonly ITagService _tagService;
    protected readonly IPageService _pageService;
    protected readonly IPostService _postService;
    protected readonly IXmlService _xmlService;

    public BaseServiceTests()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();

        _uowMock = new Mock<IUnitOfWork>();

        // Repositories
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _emailRepoMock = new Mock<IEmailRepository>();
        _websiteRepoMock = new Mock<IWebsiteRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _tagRepoMock = new Mock<ITagRepository>();
        _pageRepoMock = new Mock<IPageRepository>();
        _postRepoMock = new Mock<IPostRepository>();

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "testuser"
        };
        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var website = new Website
        {
            Id = 1,
            Name = "TestSite",
            Tagline = "Test Tagline",
            Culture = "en-US",
            UrlFormat = "[site_url]/[type]/[slug]",
            DateFormat = "yyyy-MM-dd",
            SiteUrl = "https://test.com"
        };
        _websiteRepoMock.Setup(r => r.GetWebsiteInstanceByHost(It.IsAny<string>())).Returns(website);

        _uowMock.Setup(u => u.Users).Returns(_userManagerMock.Object);
        _uowMock.Setup(u => u.Emails).Returns(_emailRepoMock.Object);
        _uowMock.Setup(u => u.Websites).Returns(_websiteRepoMock.Object);
        _uowMock.Setup(u => u.Categories).Returns(_categoryRepoMock.Object);
        _uowMock.Setup(u => u.Tags).Returns(_tagRepoMock.Object);
        _uowMock.Setup(u => u.Pages).Returns(_pageRepoMock.Object);
        _uowMock.Setup(u => u.Posts).Returns(_postRepoMock.Object);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost");
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "testuser") }));
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(context);

        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loggerMock = new Mock<ILogger>();
        _loggerFactoryMock.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

        // Services
        _cacheServiceMock = new Mock<ICacheService>();
        _cacheServiceMock.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()));
        _cacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out It.Ref<string>.IsAny)).Returns(false);

        _emailService = new Mock<EmailService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _websiteService = new Mock<WebsiteService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _categoryService = new Mock<CategoryService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _tagService = new Mock<TagService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _pageService = new Mock<PageService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _postService = new Mock<PostService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
        _xmlService = new Mock<XmlService>(_uowMock.Object, _httpContextAccessorMock.Object, _loggerFactoryMock.Object, _cacheServiceMock.Object).Object;
    }
}
