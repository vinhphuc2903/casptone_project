using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CapstoneProject.Extensions;

namespace CapstoneProject
{
    public class Program
    {
        /// <summary>
        /// Tên của microservice hiện tại
        /// </summary>
        public static readonly string ServiceName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    //.CustomConfigureAppConfiguration()
                    //.UseCustomLog(ServiceName)
                    .UseStartup<Startup>()
                    ;
                })
                ;
    }
    public static class HostExtensions
    {
        public static IWebHostBuilder UseCustomLog(this IWebHostBuilder builder, string serviceName)
        {
            try
            {
                builder
                    .ConfigureLogging((hostingContext, loggingbuilder) =>
                    {
                        loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        loggingbuilder.AddConsole();
                        loggingbuilder.AddDebug();
                    });

            }
            catch { }
            return builder;
        }

        public static IWebHostBuilder CustomConfigureAppConfiguration(this IWebHostBuilder builder)
        {
            try
            {
                builder
                    .ConfigureAppConfiguration((host, config) =>
                    {
                        config
                            .SetBasePath(host.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true)
                            .AddEnvironmentVariables();
                    })
                    ;
            }
            catch { }
            return builder;
        }
    }
}
