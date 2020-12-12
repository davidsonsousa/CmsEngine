using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CmsEngine.Ui
{
    public class Program
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
                                                           .Enrich.FromLogContext().CreateLogger();
            try
            {
                Log.Debug("Starting host");
                CreateHostBuilder(args).Build().Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string certificateName = Configuration.GetSection("certificateSettings:fileName").Value;
            string certificatePassword = Configuration.GetSection("certificateSettings:password").Value;

            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                           {
                               Log.Debug("Dev environment: Using Kestrel with port 5001");
                               webBuilder.ConfigureKestrel(options =>
                               {
                                   options.AddServerHeader = false;
                                   options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                                   {
                                       listenOptions.UseHttps(new X509Certificate2(certificateName, certificatePassword));
                                   });
                               });
                           }

                           webBuilder.UseStartup<Startup>()
                                     .UseSerilog()
                                     .UseConfiguration(Configuration); // This may affect the secrets. To be studied.
                       });
        }
    }
}
