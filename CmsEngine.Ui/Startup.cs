using System.IO;
using CmsEngine.Application.Helpers.Email;
using CmsEngine.Application.Services;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using CmsEngine.Data.Repositories;
using CmsEngine.Ui.RewriteRules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace CmsEngine.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            // Add CmsEngineContext
            services.AddDbContext<CmsEngineContext>(options => options.EnableSensitiveDataLogging(true) // TODO: Perhaps use a flag from appsettings instead of a hard-coded value
                                                                      .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<CmsEngineContext>()
                    .AddDefaultTokenProviders();

            // Add HttpContextAccessor as .NET Core doesn't have HttpContext.Current anymore
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IWebsiteRepository, WebsiteRepository>();

            //// Add services
            services.AddScoped<IService, Service>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IXmlService, XmlService>();

            //// Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            if (!Environment.IsDevelopment())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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


            // TODO: Fix this
            //// Uploaded files
            //app.ConfigureFileUpload(new FileUploadOptions
            //{
            //    Root = env.WebRootPath,
            //    Folder = "UploadedFiles"
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "UploadedFiles")),
                RequestPath = "/image"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "UploadedFiles")),
                RequestPath = "/file"
            });

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{vanityId?}");

                endpoints.MapControllerRoute(
                    name: "blog",
                    pattern: "blog/{action}/{slug?}",
                    defaults: new { controller = "Blog", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "main",
                    pattern: "",
                    defaults: new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "sitemap",
                    pattern: "sitemap",
                    defaults: new { controller = "Home", action = "Sitemap" });

                endpoints.MapControllerRoute(
                    name: "archive",
                    pattern: "archive",
                    defaults: new { controller = "Home", action = "Archive" });

                endpoints.MapControllerRoute(
                    name: "page",
                    pattern: "{slug}",
                    defaults: new { controller = "Home", action = "Page" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
