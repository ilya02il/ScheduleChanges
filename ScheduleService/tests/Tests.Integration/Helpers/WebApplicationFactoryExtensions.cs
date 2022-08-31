using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Tests.Integration.Helpers
{
    internal static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory,
            TestClaimsProvider claimsProvider)
            where T : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", opt => { });

                    services.AddScoped(_ => claimsProvider);
                });
            });
        }

        public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory,
            TestClaimsProvider claimsProvider)
            where T : class
        {
            var client = factory.WithAuthentication(claimsProvider)
                .CreateClient(new() { AllowAutoRedirect = true });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", Guid.NewGuid().ToString());

            return client;
        }
    }
}
