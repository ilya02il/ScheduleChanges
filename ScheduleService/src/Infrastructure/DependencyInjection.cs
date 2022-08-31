using Application.Common.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Files;
using Infrastructure.ReadData;
using Infrastructure.WriteData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqlServerConnection)
        {
            services.AddDbContext<EFWriteDbContext>(builder =>
                builder.UseSqlServer(
                    sqlServerConnection,
                    b => b.MigrationsAssembly(typeof(EFWriteDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped);

            services.AddScoped<IWriteDbContext>(provider => provider.GetService<EFWriteDbContext>());
            services.AddScoped<IReadDapperContext>(fact => new DapperContext(sqlServerConnection));
            services.AddTransient<ITableFileParser<ItemInfo>, WordTableFileParser<ItemInfo>>();


            return services;
        }
    }
}
