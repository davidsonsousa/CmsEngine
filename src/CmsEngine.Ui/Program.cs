var builder = WebApplication.CreateBuilder(args);

// Load json files
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile("appsettings.json")
                                           .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                                           .AddJsonFile("emailsettings.json", optional: false, reloadOnChange: true)
                                           .AddJsonFile($"emailsettings.{environment}.json", optional: true, reloadOnChange: true)
                                           .AddEnvironmentVariables()
                                           .Build();

// Initializing Logger
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                                               .Enrich.FromLogContext().CreateLogger();

// Add certificate for development
// https://dotnetplaybook.com/custom-local-domain-using-https-kestrel-asp-net-core/
if (builder.Environment.IsDevelopment())
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
builder.Services.AddDbContext<CmsEngineContext>(options => options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                                                                  .UseSqlServer(connectionString,
                                                                                o => o.MigrationsAssembly("CmsEngine.Data")
                                                                                      .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CmsEngineContext>()
                .AddDefaultTokenProviders();

// Add HttpContextAccessor as .NET Core doesn't have HttpContext.Current anymore
builder.Services.AddHttpContextAccessor();

// Add Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IWebsiteRepository, WebsiteRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

// Add services
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IWebsiteService, WebsiteService>();
builder.Services.AddScoped<IXmlService, XmlService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<ICmsEngineEmailSender, CmsEngineEmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

if (!builder.Environment.IsDevelopment())
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
