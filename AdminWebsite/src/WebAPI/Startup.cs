using CallSchedules.Service;
using ChangesLists.Service;
using Groups.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ScheduleLists.Service;
using System;
using System.Reflection;
using WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var grpcServiceUri = Configuration.GetConnectionString("ScheduleServiceConnection");
            services.AddGrpcClient<GrpcScheduleChangesLists.GrpcScheduleChangesListsClient>(options =>
            {
                options.Address = new Uri(grpcServiceUri);
            });
            services.AddGrpcClient<GrpcScheduleLists.GrpcScheduleListsClient>(options =>
            {
                options.Address = new Uri(grpcServiceUri);
            });
            services.AddGrpcClient<GrpcGroupsLists.GrpcGroupsListsClient>(options =>
            {
                options.Address = new Uri(grpcServiceUri);
            });
            services.AddGrpcClient<GrpcCallScheduleService.GrpcCallScheduleServiceClient>(options =>
            {
                options.Address = new Uri(grpcServiceUri);
            });

            services.AddScoped<GrpcScheduleChangesListsClientService>();
            services.AddScoped<GrpcGroupsClientService>();
            services.AddScoped<GrpcScheduleListsClientService>();
            services.AddScoped<GrpcCallScheduleListsClientService>();

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "University schedule service - Admin Website API",
                    Version = "v1",
                    Description = "WebAPI gateway for administrator website."
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Website API v1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
