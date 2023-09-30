using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ScheduleService.ServiceAPI.Authorization.Requirements;
using ScheduleService.ServiceAPI.GrpcClients;

namespace ScheduleService.ServiceAPI.Authorization.Handlers;

/// <summary>
/// Обработчик требования авторизации на основе JWT, проверяемого сервисом идентификации.
/// </summary>
internal sealed class JwtSourceAuthorizationHandler : AuthorizationHandler<JwtSourceAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtValidationServiceGrpcClient _jwtValidationService;
    
    public JwtSourceAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        JwtValidationServiceGrpcClient jwtValidationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtValidationService = jwtValidationService;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        JwtSourceAuthorizationRequirement requirement)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
        {
            return;
        }
        
        if (_httpContextAccessor.HttpContext is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "HttpContext can't be null."));
            return;
        }

        var token = _httpContextAccessor
            .HttpContext
            .Request
            .Headers["Authorization"]
            .ToString()
            .Split(" ")
            .Last();

        var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;
        var isTokenValid = await _jwtValidationService.ValidateJwtTokenAsync(token, cancellationToken);

        if (!isTokenValid)
        {
            context.Fail();
            return;
        }
        
        context.Succeed(requirement);
    }
}