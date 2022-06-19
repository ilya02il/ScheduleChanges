using IdentityAPI.Contracts;
using IdentityAPI.Data;
using IdentityAPI.DependencyInjection;
using IdentityAPI.GrpcServices;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace IdentityAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; init; }
        public IWebHostEnvironment Env { get; init; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Env.IsDevelopment() ? 
                Configuration.GetConnectionString("SqlServerConnection") : 
                Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                );

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddApplicationIdentity(Configuration);

            services.AddSwagger();

            services.AddScoped<IIdentityService, IdentityService>();

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
            //    options.HttpsPort = 8080;
            //});

            services.AddGrpc();
            services.AddControllers();

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
                options.SwaggerEndpoint($"/{ApiBaseRoute.BaseRoute}/docs/swagger/v1/swagger.json", "Identity Service API v1");
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
                endpoints.MapControllers();
                endpoints.MapGrpcService<JwtValidationService>();
            });
        }
    }
}
