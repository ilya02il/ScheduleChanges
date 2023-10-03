using Infrastructure.WriteData;
using ScheduleService.ServiceAPI.API.v1;
using ScheduleService.ServiceAPI.Extensions;
using ScheduleService.ServiceAPI.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureWebHost();
builder.ConfigureServices();

var application = builder.Build();

var isDevelopmentEnv = application.Environment.IsDevelopment();
var isSwaggerEnabled = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_SWAGGER_ENABLED"));

if (isDevelopmentEnv)
{
    application.UseDeveloperExceptionPage();
}

if (isDevelopmentEnv || isSwaggerEnabled)
{
    application.UseSwagger(options =>
    {
        options.RouteTemplate = "/api/{documentName}/docs/swagger.json";
    });
    application.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "";
        
        foreach (var description in application.DescribeApiVersions())
        {
            var url = $"api/{description.GroupName}/docs/swagger.json";
            var name = $"Schedule Service API {description.GroupName}";
            
            options.SwaggerEndpoint(url, name);
        }
    });
}

// application.UseCors(policyBuilder =>
// {
//     policyBuilder
//         .SetIsOriginAllowed(_ => true)
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials();
// });

application.UseAuthentication();
application.UseAuthorization();

var api = application
    .NewVersionedApi("ScheduleServiceApi")
    .MapGroup("/api/v{version:apiVersion}");

api.MapApiV1();

application.MapGrpcService<DatedSchedulesService>();

application.Run();

// Для интеграционного тестирования
// (чтобы можно было создать фабрику веб-приложения). 
public partial class Program { }