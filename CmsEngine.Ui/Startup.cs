using System.IO;
using System.Net;
using AutoMapper;
using CmsEngine.Data;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Helpers.Email;
using CmsEngine.Ui.Middleware.RewriteRules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

namespace CmsEngine.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add CmsEngineContext
            services.AddDbContextPool<CmsEngineContext>(options =>
                options.UseLazyLoadingProxies()
                       .EnableSensitiveDataLogging(true) // TODO: Perhaps use a flag from appsettings instead of a hard-coded value
                       .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<CmsEngineContext>();

            // Add HttpContextAccessor as .NET Core doesn't have HttpContext.Current anymore
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add AutoMapper
            services.AddAutoMapper();

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddRazorPagesOptions(options =>
                    {
                        options.AllowAreas = true;
                        options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // Add application services.
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            const int http301 = (int)HttpStatusCode.MovedPermanently;

            // Added compatibility to the old davidsonsousa.net
            var rewriteOptions = new RewriteOptions().Add(new RedirectToNonWwwRule(http301))
                                                     .AddRedirect("^en/(.*)", "blog/$1", http301)
                                                     .AddRedirect("^pt/(.*)", "blog/$1", http301)
                                                     .AddRedirect("^image/articles/(.*)", "image/post/$1", http301)
                                                     .AddRedirect("^image/pages/(.*)", "image/page/$1", http301)
                                                     .AddRedirect("^file/articles/(.*)", "file/post/$1", http301)
                                                     .AddRedirect("^file/pages/(.*)", "file/page/$1", http301)
                                                     .AddRedirectToHttps(http301);
            app.UseRewriter(rewriteOptions);

            // wwwroot
            app.UseStaticFiles();

            // Uploaded files
            string uploadPath = Path.Combine(env.WebRootPath, "UploadedFiles");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{vanityId?}");

                routes.MapRoute(
                    name: "blog",
                    template: "blog/{action}/{slug?}",
                    defaults: new { controller = "Blog", action = "Index" });

                routes.MapRoute(
                    name: "main",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "sitemap",
                    template: "sitemap",
                    defaults: new { controller = "Home", action = "Sitemap" });

                routes.MapRoute(
                    name: "page",
                    template: "{slug}",
                    defaults: new { controller = "Home", action = "Page" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
