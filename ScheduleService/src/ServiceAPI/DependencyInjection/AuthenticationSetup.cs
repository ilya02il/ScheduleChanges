using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ServiceAPI.DependencyInjection;

public static class AuthenticationSetup
{
    public static IServiceCollection AddServiceAuthentiction(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })/*.AddCertificate(options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.All;
                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        var claims = new[]
                        {
                                new Claim(ClaimTypes.Name,
                                    context.ClientCertificate.Subject,
                                    ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer)
                            };

                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                        context.Success();

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 403;

                        return Task.CompletedTask;
                    }
                };
            })*/
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = false,
                    RequireSignedTokens = false,
                    SignatureValidator = (token, parameters) => new JwtSecurityToken(token),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateLifetime = true
                };
            });

        return services;
    }
}