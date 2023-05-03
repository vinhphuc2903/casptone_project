using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CapstoneProject.Extensions
{
    /// <summary>
    /// Lớp mở rộng để cài đặt các cấu hình lúc build host.
    /// <para>Created at: 10/07/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Cấu hình việc ghi log và sử dụng serilog
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="builder">Đối tượng build host hiện tại</param>
        /// <param name="serviceName">Tên service cần cấu hình</param>
        /// <returns>Đối tượng build host đã cấu hình</returns>
        public static IWebHostBuilder UseCustomLog(this IWebHostBuilder builder, string serviceName)
        {
            try
            {
                builder
                    // Cấu hình ghilog thông thường
                    .ConfigureLogging((hostingContext, loggingbuilder) =>
                    {
                        loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        loggingbuilder.AddConsole();
                        loggingbuilder.AddDebug();
                    });
                    // Cấu hình sử dụng serilog
                    //.UseSerilog((builderContext, config) =>
                    //{
                    //    config
                    //        .MinimumLevel.Information()
                    //        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                    //        .Enrich.WithProperty("ApplicationContext", serviceName)
                    //        .Enrich.FromLogContext()
                    //        .WriteTo.Console()
                    //        .WriteTo.File(Path.Combine("logs", $"{serviceName}.log"), rollingInterval: RollingInterval.Day)
                    //        ;
                    //});

            }
            catch { }

            return builder;
        }

        /// <summary>
        /// Cấu hình việc sử dụng file config của hệ thống
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="builder">Đối tượng build host hiện tại</param>
        /// <param name="serviceName">Tên service cần cấu hình</param>
        /// <returns>Đối tượng build host đã cấu hình</returns>
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
