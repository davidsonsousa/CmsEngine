using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CmsEngine.Ui
{
    public static class Program
    {
        public static IConfiguration Configuration
        {
            get
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                 .AddJsonFile("appsettings.json")
                                                 .AddJsonFile($"appsettings.{environment}.json", optional: true)
                                                 .AddEnvironmentVariables()
                                                 .Build();
            }
        }

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                  .Enrich.FromLogContext()
                                                  .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       var env = hostingContext.HostingEnvironment;
                       config.AddJsonFile("emailsettings.json")
                             .AddJsonFile($"emailsettings.{env.EnvironmentName}.json", optional: true);

                       config.AddEnvironmentVariables();
                   });
    }
}
