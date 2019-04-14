using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
                                                       .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                                                       .AddJsonFile("emailsettings.json", optional: false, reloadOnChange: true)
                                                       .AddJsonFile($"emailsettings.{environment}.json", optional: true, reloadOnChange: true)
                                                       .AddJsonFile("certificate.json", optional: true, reloadOnChange: true)
                                                       .AddJsonFile($"certificate.{environment}.json", optional: true, reloadOnChange: true)
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
                var certificateSettings = Configuration.GetSection("certificateSettings");
                string certificateName = certificateSettings.GetValue<string>("fileName");
                string certificatePassword = certificateSettings.GetValue<string>("password");

                Log.Information("Starting host");
                CreateWebHostBuilder(args)
                    .UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                        options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps(new X509Certificate2(certificateName, certificatePassword));
                        });
                    })
                    .Build()
                    .Run();
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .UseSerilog();
        }
    }
}
