using Application;
using Infrastructure;
using JwtValidation.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceAPI.DependencyInjection;
using ServiceAPI.GrpcClients;
using ServiceAPI.GrpcServices;
using System;
using System.Reflection;

namespace ServiceAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlServerConnection, grpcJwtValidationConnection;

            if (Env.IsDevelopment())
            {
                sqlServerConnection = Configuration.GetConnectionString("DefaultConnection");
                grpcJwtValidationConnection = Configuration.GetConnectionString("GrpcJwtValidationServiceConnection");
            }

            else
            {
                sqlServerConnection = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION") ?? string.Empty;
                grpcJwtValidationConnection = Environment.GetEnvironmentVariable("GRPC_JWT_VALIDATION_CONNECTION") ?? string.Empty;
            }

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddApplication();
            services.AddInfrastructure(sqlServerConnection);

            services.AddGrpc();

            services.AddGrpcClient<GrpcJwtValidationService.GrpcJwtValidationServiceClient>(options =>
            {
                options.Address = new Uri(grpcJwtValidationConnection);
            });
            services.AddScoped<JwtValidationServiceGrpcClient>();

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
            //    options.HttpsPort = 80;
            //});

            services.AddControllers();

            services.AddSwagger();
            services.AddServiceAuthentiction();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = ApiBaseRoute.BaseRoute + "/docs/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/{ApiBaseRoute.BaseRoute}/docs/swagger/v1/swagger.json", "Admin Website API v1");
                options.RoutePrefix = ApiBaseRoute.BaseRoute + "/docs/swagger";
            });

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)// allow any origin
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DatedSchedulesService>();
                endpoints.MapControllers();
            });
        }
    }
}
