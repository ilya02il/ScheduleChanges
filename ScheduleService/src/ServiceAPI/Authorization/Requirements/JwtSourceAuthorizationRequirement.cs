using Microsoft.AspNetCore.Authorization;

namespace ScheduleService.ServiceAPI.Authorization.Requirements;

/// <summary>
/// Требование авторизации на основе JWT, проверяемого сервисом идентификации.
/// </summary>
internal sealed class JwtSourceAuthorizationRequirement : IAuthorizationRequirement { }