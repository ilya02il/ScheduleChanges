using Application.Common.Interfaces;
using Infrastructure.EF;
using Infrastructure.GrpcScheduleClient;
using Infrastructure.RedisCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(
                    configuration.GetConnectionString("SqlServerConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });

            services.AddGrpcClient<GrpcSchedule.GrpcScheduleClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("GrpcScheduleServiceConnection"));
            });

            services.AddScoped<IApplicationDbContext>(provider => 
                provider.GetService<ApplicationDbContext>()
                );

            services.AddScoped<IDistributedCacheWrapper, RedisCacheWrapper>();
            services.AddScoped<IGrpcScheduleClientWrapper, GrpcScheduleClientWrapper>();

            return services;
        }
    }
}
