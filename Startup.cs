using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using CapstoneProject.Extensions;
using Microsoft.OpenApi.Models;
using CapstoneProject.Databases;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CapstoneProject.Services;
using CapstoneProject.AutofacModules;

namespace CapstoneProject
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
            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ReportApiVersions = true;
            //});

            // services.AddApiVersioning(options =>
            // {
            //     options.DefaultApiVersion = new ApiVersion(1, 0);
            //     options.AssumeDefaultVersionWhenUnspecified = true;
            //     options.ReportApiVersions = true;
            //     options.ApiVersionReader = ApiVersionReader.Combine(
            //         new QueryStringApiVersionReader("api-version"),
            //         new HeaderApiVersionReader("api-version"));
            // });
            services
                .AddCustomApiVersioning(Configuration)
                //.AddCustomHealthChecks(Configuration, Program.ServiceName)
                .AddCustomCors(Configuration)
                .AddControllers()
                ;
            services.AddCustomAuthentication(Configuration);

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder =>
            //        {
            //            builder.AllowAnyOrigin()
            //                   .AllowAnyHeader()
            //                   .AllowAnyMethod();
            //        });
            //});

            services.AddHttpContextAccessor();
            services
                    .AddApiVersioning(options =>
                    {
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        //options.ApiVersionReader = ApiVersionReader.Combine(
                        //    new QueryStringApiVersionReader("api-version")
                            //new HeaderApiVersionReader("x-api-version")
                        //);
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                    })
                ;
            services.AddCustomSwagger(Configuration, Program.ServiceName);

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            // Add controllers and other services
            services.AddControllers();

            // Add authentication and authorization
            // services.AddAuthentication();
            // services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            // Add Swagger documentation
            services.AddSwaggerGen();
            // Add authen
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Jwt:Issuer"],
            //        ValidAudience = Configuration["Jwt:Issuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            //    c.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "basic",
            //        In = ParameterLocation.Header,
            //        Name = "Authorization",
            //        Description = "Basic authentication header"
            //    });

            //    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.OAuth2,
            //        Flows = new OpenApiOAuthFlows
            //        {
            //            AuthorizationCode = new OpenApiOAuthFlow
            //            {
            //                AuthorizationUrl = new Uri("https://example.com/auth"),
            //                TokenUrl = new Uri("https://example.com/token"),
            //                Scopes = new Dictionary<string, string>
            //        {
            //            { "read", "Read access" },
            //            { "write", "Write access" }
            //        }
            //            }
            //        }
            //    });

            //    //c.OperationFilter<SecurityRequirementsOperationFilter>();
            //});
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IMediaService, MediaService>();
            // Add db
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("Server=localhost;Database=CasptonePrjDB;User Id=sa;Password=123456aA@$;TrustServerCertificate=true;"));
        }

        // ConfigureContainer is where you can register things directly with Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here.
            builder.RegisterModule(new AutofacModules.AppModule());
            builder.RegisterModule(new AutofacModules.EmployeeModule());
            builder.RegisterModule(new AutofacModules.UserModule());
            builder.RegisterModule(new AutofacModules.FilmModule());
            builder.RegisterModule(new AutofacModules.ShowTimeModule());
            builder.RegisterModule(new AutofacModules.CustomerModule()); 
            builder.RegisterModule(new AutofacModules.CinemaRoomModule());
            builder.RegisterModule(new AutofacModules.OrderModule());
            builder.RegisterModule(new AutofacModules.ReportModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var connection = String.Empty;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            // Use Swagger documentation
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            // Use authentication and authorization
            app.UseAuthentication(); // Bổ sung middleware xác thực
            app.UseAuthorization(); // Bổ sung middleware phân quyền
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app
                .UseRouting()
                .UseCustomCors(Program.ServiceName)
                .UseCustomAuth(Program.ServiceName)
                .UseEndpoints(
                    endpoints =>
                    {
                        endpoints.MapControllers();
                    }
                );
        }

    }
}
