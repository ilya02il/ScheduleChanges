using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ServiceAPI;
using ServiceAPI.Extensions;
using ServiceAPI.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureWebHost();
builder.ConfigureServices();

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application.UseDeveloperExceptionPage();
}

application.UseSwagger(options =>
{
    options.RouteTemplate = ApiBaseRoute.BaseRoute + "/docs/swagger/{documentName}/swagger.json";
});
application.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/{ApiBaseRoute.BaseRoute}/docs/swagger/v1/swagger.json", "Admin Website API v1");
    options.RoutePrefix = ApiBaseRoute.BaseRoute + "/docs/swagger";
});

application.UseRouting();

application.UseCors(policyBuilder =>
{
    policyBuilder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

application.UseAuthentication();
application.UseAuthorization();

application.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DatedSchedulesService>();
    endpoints.MapControllers();
});

application.Run();