using System.Reflection;
using Application;
using Asp.Versioning;
using Infrastructure;
using JwtValidation.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ScheduleService.ServiceAPI.Authorization.Handlers;
using ScheduleService.ServiceAPI.DependencyInjection;
using ScheduleService.ServiceAPI.GrpcClients;
using ScheduleService.ServiceAPI.OptionsConfigurators;

namespace ScheduleService.ServiceAPI.Extensions;

/// <summary>
/// Статический класс с методами расширения для конфигурации веб-приложения
/// </summary>
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Сконфигурировать хост веб-приложения.
    /// </summary>
    /// <param name="builder">Билдер хоста веб-приложения.</param>
    public static void ConfigureWebHost(this WebApplicationBuilder builder)
    {
        var grpcPort = !builder.Environment.IsDevelopment()
            ? Convert.ToInt32(Environment.GetEnvironmentVariable("GRPC_PORT"))
            : 5200;

        var webApiPort = !builder.Environment.IsDevelopment()
            ? Convert.ToInt32(Environment.GetEnvironmentVariable("WEB_API_PORT"))
            : 5000;

        builder.WebHost.UseKestrel(options =>
        {
            options.ListenAnyIP(grpcPort, opt =>
            {
                opt.Protocols = HttpProtocols.Http2;
            });
            options.ListenAnyIP(webApiPort, opt =>
            {
                opt.Protocols = HttpProtocols.Http1;
            });
        });
    }

    /// <summary>
    /// Сконфигурировать сервисы веб-приложения.
    /// </summary>
    /// <param name="builder">Билдер хоста веб-приложения.</param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var sqlServerConnection = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("DefaultConnection")
            : Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION");

        var grpcValidationConnection = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("GrpcJwtValidationServiceConnection")
            : Environment.GetEnvironmentVariable("GRPC_JWT_VALIDATION_CONNECTION");

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(sqlServerConnection ?? string.Empty);

        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddScoped<IAuthorizationHandler, JwtSourceAuthorizationHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, RoleBasedAuthorizationHandler>();

        builder.Services.AddGrpc();
        builder
            .Services
            .AddGrpcClient<GrpcJwtValidationService.GrpcJwtValidationServiceClient>(options =>
            {
                options.Address = new Uri(grpcValidationConnection ?? string.Empty);
            });
        builder.Services.AddScoped<JwtValidationServiceGrpcClient>();

        builder.Services.AddEndpointsApiExplorer();

        builder
            .Services
            .AddSwaggerGen(options =>
            {
                var xmlDocsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlDocsFilePath = Path.Combine(AppContext.BaseDirectory, xmlDocsFileName);
                
                options.IncludeXmlComments(xmlDocsFilePath);
            });

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        builder
            .Services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
                options.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();
        
        builder.Services.AddServiceAuthentiction();
        builder.Services.AddCors();
    } 
}