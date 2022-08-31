﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Application.Tests.Integration.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Remove<TService>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);

            return services;
        }
    }
}
