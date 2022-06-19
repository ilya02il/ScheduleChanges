using Application.Common.Interfaces;
using Infrastructure.EF;
using Infrastructure.GrpcScheduleClient;
using Infrastructure.RedisCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DatedSchedules.Service;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            string sqlServerConnection, redisConnection, grpcScheduleSrvConnection;

            if (isDevelopment)
            {
                sqlServerConnection = configuration.GetConnectionString("SqlServerConnection");
                redisConnection = configuration.GetConnectionString("RedisConnection");
                grpcScheduleSrvConnection = configuration.GetConnectionString("GrpcScheduleServiceConnection");
            }

            else
            {
                sqlServerConnection = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION");
                redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
                grpcScheduleSrvConnection = Environment.GetEnvironmentVariable("GRPC_SCHEDULE_SRV_CONNECTION");
            }

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(
                    sqlServerConnection,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConnection;
            });

            services.AddGrpcClient<GrpcSchedule.GrpcScheduleClient>(options =>
            {
                options.Address = new Uri(grpcScheduleSrvConnection);
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
