var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

// Load json files
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile("appsettings.json")
                                           .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: isDevelopment)
                                           .AddJsonFile("emailsettings.json", optional: false, reloadOnChange: isDevelopment)
                                           .AddJsonFile($"emailsettings.{environment}.json", optional: true, reloadOnChange: isDevelopment)
                                           .AddEnvironmentVariables();

// Initializing Logger
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                                               .Enrich.FromLogContext().CreateLogger();

// Add certificate for development
// https://dotnetplaybook.com/custom-local-domain-using-https-kestrel-asp-net-core/
if (isDevelopment)
{
    var certificatePassword = builder.Configuration["CertPassword"];

    Log.Debug("Dev environment: Using Kestrel with port 5001");

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
        options.Listen(IPAddress.Loopback, 5001, listenOptions =>
        {
            listenOptions.UseHttps(X509CertificateLoader.LoadPkcs12FromFile("cmsengine.test.pfx", certificatePassword));
            //listenOptions.UseHttps();
        });
    })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
        });
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CmsEngineContext>(options => options.EnableSensitiveDataLogging(isDevelopment)
                                                                  .UseSqlServer(connectionString,
                                                                                o => o.MigrationsAssembly("CmsEngine.Data")
                                                                                      .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CmsEngineContext>()
                .AddDefaultTokenProviders();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024;
    options.CompactionPercentage = 0.2;
});
builder.Services.Configure<MemoryCacheOptions>(builder.Configuration.GetSection("MemoryCache"));
builder.Services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<MemoryCacheOptions>>().Value);

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add Repositories
builder.Services.TryAddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.TryAddScoped<IPageRepository, PageRepository>();
builder.Services.TryAddScoped<IPostRepository, PostRepository>();
builder.Services.TryAddScoped<ITagRepository, TagRepository>();
builder.Services.TryAddScoped<IWebsiteRepository, WebsiteRepository>();
builder.Services.TryAddScoped<IEmailRepository, EmailRepository>();

// Add services
builder.Services.TryAddScoped<ICacheService, MemoryCacheService>();
builder.Services.TryAddScoped<IService, Service>();
builder.Services.TryAddScoped<ICategoryService, CategoryService>();
builder.Services.TryAddScoped<IPageService, PageService>();
builder.Services.TryAddScoped<IPostService, PostService>();
builder.Services.TryAddScoped<ITagService, TagService>();
builder.Services.TryAddScoped<IWebsiteService, WebsiteService>();
builder.Services.TryAddScoped<IXmlService, XmlService>();
builder.Services.TryAddScoped<IEmailService, EmailService>();

// Add Unit of Work
builder.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<ICmsEngineEmailSender, CmsEngineEmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

if (!isDevelopment)
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
        options.HttpsPort = 443;
    });
}

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

const int http301 = StatusCodes.Status301MovedPermanently;

// Added compatibility with the old davidsonsousa.net
var rewriteOptions = new RewriteOptions().Add(new RedirectToNonWwwRule(http301))
                                         .Add(new RedirectLowerCaseRule(http301))
                                         .AddRedirect("^en/(.*)", "blog/$1", http301)
                                         .AddRedirect("^pt/(.*)", "blog/$1", http301)
                                         .AddRedirect("^image/articles/(.*)", "image/post/$1", http301)
                                         .AddRedirect("^image/pages/(.*)", "image/page/$1", http301)
                                         .AddRedirect("^file/articles/(.*)", "file/post/$1", http301)
                                         .AddRedirect("^file/pages/(.*)", "file/page/$1", http301);
app.UseRewriter(rewriteOptions);
app.UseHttpsRedirection();

// wwwroot
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "UploadedFiles")),
    RequestPath = "/image"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "UploadedFiles")),
    RequestPath = "/file"
});

app.UseSecurityHeaders(new SecurityHeadersBuilder().AddDefaultSecurePolicy());

app.UseCookiePolicy();

app.UseRouting();

app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{vanityId?}");

app.MapControllerRoute(
    name: "blog",
    pattern: "blog/{action}/{slug?}",
    defaults: new { controller = "Blog", action = "Index" });

app.MapControllerRoute(
    name: "main",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "sitemap",
    pattern: "sitemap",
    defaults: new { controller = "Home", action = "Sitemap" });

app.MapControllerRoute(
    name: "archive",
    pattern: "archive",
    defaults: new { controller = "Home", action = "Archive" });

// Deprecated route, redirecting to Index
app.MapControllerRoute(
    name: "contact",
    pattern: "contact",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "error",
    pattern: "error",
    defaults: new { controller = "Error", action = "Index" });

app.MapControllerRoute(
    name: "page",
    pattern: "{slug}",
    defaults: new { controller = "Home", action = "Page" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

Log.CloseAndFlush();