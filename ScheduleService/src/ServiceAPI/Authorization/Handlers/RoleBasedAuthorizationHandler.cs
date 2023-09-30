using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ScheduleService.ServiceAPI.Authorization.Requirements;

namespace ScheduleService.ServiceAPI.Authorization.Handlers;

/// <summary>
/// Обработчик требования авторизации на основе ролей пользователя.
/// </summary>
internal sealed class RoleBasedAuthorizationHandler : AuthorizationHandler<RoleBasedAuthenticationRequirement>
{
    private readonly HttpContext? _httpContext;

    public RoleBasedAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleBasedAuthenticationRequirement requirement)
    {
        var hasRole = context.User
            .HasClaim(claim =>
            {
                var isRoleClaimType = claim.Type is ClaimTypes.Role;
                var isInRole = requirement.Roles.TryGetValue(claim.Value, out _);

                return isRoleClaimType && isInRole;
            });

        if (hasRole)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        var authenticationFailureReason = new AuthorizationFailureReason(
            this,
            $"The user must have one of the `{string.Join(", ", requirement.Roles)}` roles."
        );

        if (_httpContext is not null)
        {
            _httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
            
        context.Fail(authenticationFailureReason);
        return Task.CompletedTask;
    }
}