using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAPI.GrpcClients;
using System.Threading.Tasks;

namespace ServiceAPI.Attributes;

public class AuthorizeOnJwtSourceAttribute 
    : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public AuthorizeOnJwtSourceAttribute(string? roles = default)
    {
        this.Roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            var authorizationService = context.HttpContext
                .RequestServices
                .GetRequiredService<JwtValidationServiceGrpcClient>();

            string token = context.HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()[7..];

            bool isTokenValid = await authorizationService.ValidateJwtTokenAsync(token,
                context.HttpContext.RequestAborted);

            if (!isTokenValid)
                context.Result = new UnauthorizedResult();
        }
    }
}