using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CmsEngine.Ui
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              var env = hostingContext.HostingEnvironment;
                              config.AddJsonFile("emailsettings.json")
                                    .AddJsonFile($"emailsettings.{env.EnvironmentName}.json", optional: true);
                              config.AddEnvironmentVariables();
                          })
                          .Build();
        }
    }
}
