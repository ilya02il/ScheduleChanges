using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAPI.GrpcClients;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAPI.Attributes
{
    public class AuthorizeOnJwtSourceAttribute 
        : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated ?? false) // -> first, we need to check if we have some toeken
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
}
