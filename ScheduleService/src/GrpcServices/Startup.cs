using Application;
using GrpcAPI.Profiles;
using GrpcAPI.Services;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace GrpcAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddGrpc(options =>
            {
                options.MaxReceiveMessageSize = 11 * 1024 * 1024;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DatedSchedulesService>();
                endpoints.MapGrpcService<ScheduleChangesListsService>();
                endpoints.MapGrpcService<ScheduleListsService>();
                endpoints.MapGrpcService<GroupsService>();
                endpoints.MapGrpcService<CallScheduleListsService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("404");
                });
            });
        }
    }
}
