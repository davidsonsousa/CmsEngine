using AutoMapper;
using CmsEngine.Data;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Helpers.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace CmsEngine.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static readonly LoggerFactory loggerFactory = new LoggerFactory(new[] {
                                                                new ConsoleLoggerProvider((category, level) =>
                                                                                    category == DbLoggerCategory.Database.Command.Name
                                                                                 && level == LogLevel.Information, true) });

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
                       .UseLoggerFactory(loggerFactory)
                       .EnableSensitiveDataLogging(true) // TODO: Perhaps use a flag from appsettings instead of a hard-coded value
                       .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<CmsEngineContext>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // Add HttpContextAccessor as .NET Core doesn't have HttpContext.Current anymore
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add AutoMapper
            services.AddAutoMapper();

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{vanityId?}");

                routes.MapRoute(
                    name: "blogRoute",
                    template: "{controller=Home}/{action=Index}/{slug?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
