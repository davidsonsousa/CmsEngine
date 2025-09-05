using CmsEngine.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

    protected readonly Mock<ICacheService> _cacheServiceMock;
    protected readonly IEmailService _emailService;
    protected readonly IWebsiteService _websiteService;
    protected readonly ICategoryService _categoryService;

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

        _uowMock.Setup(u => u.Users).Returns(_userManagerMock.Object);
        _uowMock.Setup(u => u.Emails).Returns(_emailRepoMock.Object);
        _uowMock.Setup(u => u.Websites).Returns(_websiteRepoMock.Object);
        _uowMock.Setup(u => u.Categories).Returns(_categoryRepoMock.Object);

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
    }
}
