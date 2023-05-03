using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Http;

namespace CapstoneProject.Extensions
{
    /// <summary>
    /// Lớp mở rộng để cấu hình app cho 1 microservice.
    /// <para>Created at: 10/07/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// Cấu hình việc sử dụng swagger cho app
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="app">Application hiện tại của service</param>
        /// <param name="serviceName">Tên microservice đang cấu hình</param>
        /// <param name="versions">Các version cần tạo</param>
        /// <returns>Application đang câu hình</returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, string serviceName, string[] versions = null)
        {
            // Lấy dịch vụ ghi log
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(serviceName);
            try
            {
                // Khởi tạo version mặc định nếu không được truyền version
                if (versions == null)
                {
                    versions = new string[] { "v1.0" };
                }

                // Cấu hình cho swagger
                app
                    .UseSwagger(c =>
                    {
                        c.SerializeAsV2 = true;
                    })
                    .UseSwaggerUI(c =>
                    {
                        foreach (string version in versions)
                        {
                            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{serviceName} {version}");
                        }
                        c.OAuthClientId(serviceName);
                        c.OAuthAppName(serviceName);
                        c.InjectStylesheet("/public/css/custom-swagger.css?v=1");
                        c.InjectJavascript("/public/assets/jquery/js/jquery-3.2.1.min.js");
                        c.InjectJavascript("/public/js/common/jquery-cookie.js");
                        c.InjectJavascript("/public/js/common/signalr.js");
                        c.InjectJavascript("/public/js/common/secure.js");
                        c.InjectJavascript("/public/js/common/common.js");
                        c.InjectJavascript("/public/js/common/custom-swagger.js?v=10");
                    })
                    ;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return app;
        }

        /// <summary>
        /// Cấu hình việc sử dụng kiểm tra trạng thái microservice cho app
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="app">Application hiện tại của service</param>
        /// <param name="serviceName">Tên microservice đang cấu hình</param>
        /// <returns>Application đang câu hình</returns>
        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app, string serviceName = "")
        {
            // Lấy dịch vụ ghi log
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(serviceName);
            try
            {
                // Cấu hình việc tự kiểm tra của microservice
                app
                    .UseHealthChecks("/liveness", new HealthCheckOptions
                    {
                        Predicate = r => r.Name.Contains("self"),
                        // cài đặt status trả về theo trạng thái microservice
                        ResultStatusCodes =
                        {
                            [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                        }
                    })
                    ;
                // Cấu hình cho kiểm tra tất cả các dịch vụ đã cài đặt
                app
                    .UseHealthChecks("/hc", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    })
                    ;
                // Cấu hình sử dụng giao diện kết quả kiểm tra
                //app
                //    .UseHealthChecksUI(setup =>
                //    {
                //        setup.ApiPath = "/hc";
                //        setup.UIPath = "/hc-ui";
                //    })
                //    ;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return app;
        }

        /// <summary>
        /// Cấu hình việc sử dụng cross origin cho app
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="app">Application hiện tại của service</param>
        /// <param name="serviceName">Tên microservice đang cấu hình</param>
        /// <param name="corsName">Tên của cài đặt cross orgin cần sử dụng</param>
        /// <returns>Application đang câu hình</returns>
        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app, string serviceName = "", string corsName = "CorsPolicy")
        {
            // Lấy dịch vụ ghi log
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(serviceName);
            try
            {
                app.UseCors(corsName);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return app;
        }

        public static IApplicationBuilder UseCustomAuth(this IApplicationBuilder app, string serviceName = "")
        {
            // Lấy dịch vụ ghi log
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(serviceName);
            try
            {
                app.UseAuthentication();

                app.UseAuthorization();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return app;
        }
    }
}
