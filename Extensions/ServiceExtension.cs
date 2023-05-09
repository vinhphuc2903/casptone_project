using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Autofac;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json.Linq;
using CapstoneProject.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CapstoneProject.Extensions
{
    /// <summary>
    /// Lớp mở rộng để cấu hình các dịch vụ cần thiết cho 1 microservice
    /// <para>Created at: 10/07/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Cấu hình cross orgin cho API
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <param name="corsName">Tên cấu hình cross orgin </param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration, string corsName = "CorsPolicy")
        {
            services.AddCors(options =>
            {
                options
                    .AddPolicy(corsName,
                        builder => builder
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()

                    )
                    ;
            });

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng version cho API
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                // Thêm api version
                // services
                //     .AddApiVersioning(options =>
                //     {
                //         options.ReportApiVersions = true;
                //         options.AssumeDefaultVersionWhenUnspecified = true;
                //         options.ApiVersionReader = ApiVersionReader.Combine(
                //             new QueryStringApiVersionReader(),
                //             new HeaderApiVersionReader("x-api-version")
                //         );
                //         options.DefaultApiVersion = new ApiVersion(1, 0);
                //     })
                // ;
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng Swagger để tạo API doc và test API
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <param name="serviceName">Tên microservice cần cấu hình</param>
        /// <param name="version">Version của API doc</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration, string serviceName, string version = "v1.0")
        {
            try
            {
                services
                    .AddSwaggerGen(options =>
                    {
                        //Set một số thông tin cho API doc
                        //options.SwaggerDoc(version, new OpenApiInfo
                        //{
                        //    Title = $"{serviceName} HTTP API",
                        //    Version = version,
                        //    Description = $"The {serviceName} Service HTTP API",
                        //    TermsOfService = new Uri("https://outfiz.vn/terms"),
                        //    Contact = new OpenApiContact
                        //    {
                        //        Name = "OutFiz.vn",
                        //        Email = "developer.contact@outfiz.vn",
                        //        Url = new Uri("https://developer.outfiz.vn"),
                        //    },
                        //    License = new OpenApiLicense
                        //    {
                        //        Name = "Use under LICX",
                        //        Url = new Uri("https://developer.outfiz.vn/license"),
                        //    }
                        //});
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "ToDo API",
                            Description = "An ASP.NET Core Web API for managing ToDo items",
                            TermsOfService = new Uri("https://example.com/terms"),
                            Contact = new OpenApiContact
                            {
                                Name = "Example Contact",
                                Url = new Uri("https://example.com/contact")
                            },
                            License = new OpenApiLicense
                            {
                                Name = "Example License",
                                Url = new Uri("https://example.com/license")
                            }
                        });

                        //  Nếu có 2 API trùng nhau thì lấy API đầu tiên để đưa vào API doc
                        //options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                        string Token = Helpers.GetCookie("TokenAPI");

                        // Cấu hình xác thực cho API doc
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"" + (String.IsNullOrEmpty(Token) ? "" : (". Curent token: Bearer " + Token)),
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });
                        // options.OperationFilter<AuthorizeCheckOperationFilter>();

                        // Set the comments path for the Swagger JSON and UI.
                        //var xmlFile = $"{serviceName}.xml";
                        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        //options.IncludeXmlComments(xmlPath);
                        // using System.Reflection;
                        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                        options.OperationFilter<SwaggerParameterFilter>();
                        options.CustomSchemaIds(x => x.FullName);
                    }
                )
                ;

            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng xác thực cho API
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                /// <summary>
                /// Từ khoá trong file config chứa các thông tin authentication
                /// </summary>
                string C_JWT = "Audience";
                /// <summary>
                /// Khoá bí mật dùng để mã hoá và giải mã thông tin đăng nhập
                /// </summary>
                string C_JWT_SECRET_KEY = "Secret";
                /// <summary>
                /// Đối tượng phát hành token
                /// </summary>
                string C_JWT_ISSUER = "Iss";
                /// <summary>
                /// Đối tượng sử dụng token
                /// </summary>
                string C_JWT_AUDIENCE = "Aud";
                // Lấy thông tin cấu hình xác thực từ file appsetting.json
                // Nếu không có cài đặt sẽ lấy giá trị mặc định
                var audienceConfig = configuration.GetSection(C_JWT);
                // Lấy khoá bí mật
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig[C_JWT_SECRET_KEY] != null ? audienceConfig[C_JWT_SECRET_KEY] : Constants.JWT_SECRET_KEY));
                // Cấu hình thong tin để xác thực token
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig[C_JWT_ISSUER] != null ? audienceConfig[C_JWT_ISSUER] : Constants.JWT_ISSUER,
                    ValidateAudience = true,
                    ValidAudience = audienceConfig[C_JWT_AUDIENCE] != null ? audienceConfig[C_JWT_AUDIENCE] : Constants.JWT_AUD,
                    //ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    //RequireExpirationTime = true,
                };

                // Thêm dịch vụ xác thực vào các dịch vụ hiện tại
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["Token"];
                                var path = context.HttpContext.Request.Path;
                                if (!String.IsNullOrEmpty(accessToken.ToString()) &&
                                    (path.ToString().StartsWith("/hub/")))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                        options.TokenValidationParameters = tokenValidationParameters;
                    });
                ;
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình kiểm tra tình trạng của microservice
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <param name="serviceName">Tên microservice đang cấu hình</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        //public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration, string serviceName = "")
        //{
        //    try
        //    {
        //        // Thêm giao diện cho việc hiển thị kết quả kiểm tra
        //        services
        //            .AddHealthChecksUI()
        //            .AddInMemoryStorage()
        //            ;

        //        var hcBuilder = services
        //            .AddHealthChecks()
        //            ;

        //        // Kiểm tra chính dịch vụ đang cấu hình
        //        hcBuilder
        //            .AddCheck("self", () => HealthCheckResult.Healthy())
        //            ;

        //        // Kiểm tra đến kết nối SQL
        //        hcBuilder
        //            .AddNpgSql(
        //                configuration["ConnectionString"] ?? Constants.CONNECTION_STRING,
        //                name: serviceName + ".DB-check",
        //                tags: new string[] { "progesssql" })
        //            ;
        //    }
        //    catch { }

        //    return services;
    }

    /// <summary>
    /// Thêm một số cài đặt cấu hình khác
    /// <para>Created at: 10/07/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    /// <param name="services">Các dịch vụ của API hiện tại</param>
    /// <param name="configuration">Các cấu hình API hiện tại</param>
    /// <returns>Dịch vụ sau khi đã cài đặt</returns>
    //public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    //{
    //    try
    //    {
    //        // Thêm dịch vụ tuỳ chọn
    //        services
    //            .AddOptions()
    //            ;
    //        // Cấu hình giá trị trả về cho một số lỗi nhất định
    //        services
    //            .Configure<ApiBehaviorOptions>(options =>
    //            {
    //                options.InvalidModelStateResponseFactory = context =>
    //                {
    //                    var problemDetails = new ValidationProblemDetails(context.ModelState)
    //                    {
    //                        Instance = context.HttpContext.Request.Path,
    //                        Status = StatusCodes.Status400BadRequest,
    //                        Detail = "Please refer to the errors property for additional details."
    //                    };

    //                    return new BadRequestObjectResult(problemDetails)
    //                    {
    //                        ContentTypes = { "application/problem+json", "application/problem+xml" }
    //                    };
    //                };
    //            })
    //            ;
    // If using Kestrel:
    //services.Configure<KestrelServerOptions>(options =>
    //{
    //    options.AllowSynchronousIO = true;
    //});

    // If using IIS:
    //services.Configure<IISServerOptions>(options =>
    //{
    //    options.AllowSynchronousIO = true;
    //});
    //        }
    //        catch { }

    //        return services;
    //    }
    //}

    /// <summary>
    /// Thuộc tính để thêm vào property để bỏ qua khi render swagger UI
    /// <para>Created at: 08/08/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }

    /// <summary>
    /// Lọc các tham số của API sẽ render trên UI, bỏ qua các tham số trên dường dẫn (api_version) và thêm các giá trị mặc định cho các tham số
    /// <para>Created at: 08/08/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    public class SwaggerParameterFilter : IOperationFilter
    {
        /// <summary>
        /// Ghi đè phương thức để lọc tham số
        /// <para>Created at: 08/08/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Lấy các tham số cần bỏ qua
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<SwaggerExcludeAttribute>() != null));

            if (ignoredProperties.Any())
            {
                // Nếu có tham số cần bỏ qua thì tiến hành xoá khỏi các tham số sẽ render
                foreach (var property in ignoredProperties)
                {
                    operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture)).ToList();
                }

            }

            // Xoá tham số api version trên dường dẫn
            operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals("v", StringComparison.InvariantCulture) && !p.Name.Equals("api_version", StringComparison.InvariantCulture)).ToList();

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            // Thêm tham số là api version với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "api_version",
                Description = "Version của API cần gọi",
                In = ParameterLocation.Path,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("1.0") },
                Required = true
            });

            // Thêm tham số là x-requestid với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-requestid",
                Description = "Id để định danh cho request này (mỗi lần request hãy truyền một uuid khác nhau)",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("593a8bfb-f53e-42ad-ae96-75e2ae803f1a") },
                Required = true
            });

            // Thêm tham số là x-apikey với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-apikey",
                Description = "API key để xác thực nguồn gọi API",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("PnVdWXApSHQlUiJDey14aFU4TVVROT1aP0tAOVhwSGE") },
                Required = true
            });

            // Thêm tham số là x-apikey với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-serial-number",
                Description = "Số serial của thiết bị đang sử dụng",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("G6TZL899N70M") },
                Required = false
            });

            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();


            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}
