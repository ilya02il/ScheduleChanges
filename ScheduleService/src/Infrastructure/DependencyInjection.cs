using Application.Common.Interfaces;
using Domain.ValueObjects;
using Infrastructure.EF;
using Infrastructure.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqlServerConnection)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(
                    sqlServerConnection,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)),
                ServiceLifetime.Scoped);

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<ITableFileParser<ItemInfo>, WordTableFileParser<ItemInfo>>();


            return services;
        }
    }
}
