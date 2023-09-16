using System;
using System.Reflection;
using Application;
using Infrastructure;
using JwtValidation.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceAPI.DependencyInjection;
using ServiceAPI.GrpcClients;

namespace ServiceAPI.Extensions;

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
            : Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION") ?? string.Empty;

        var grpcValidationConnection = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("GrpcJwtValidationServiceConnection")
            : Environment.GetEnvironmentVariable("GRPC_JWT_VALIDATION_CONNECTION") ?? string.Empty;

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(sqlServerConnection);

        builder.Services.AddGrpc();
        builder
            .Services
            .AddGrpcClient<GrpcJwtValidationService.GrpcJwtValidationServiceClient>(options =>
            {
                options.Address = new Uri(grpcValidationConnection);
            });
        builder.Services.AddScoped<JwtValidationServiceGrpcClient>();

        builder.Services.AddControllers();

        builder.Services.AddSwagger();
        builder.Services.AddServiceAuthentiction();

        builder.Services.AddCors();
    } 
}