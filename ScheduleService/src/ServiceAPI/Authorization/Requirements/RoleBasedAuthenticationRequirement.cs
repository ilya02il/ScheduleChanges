using Microsoft.AspNetCore.Authorization;
using ScheduleService.ServiceAPI.Enums;

namespace ScheduleService.ServiceAPI.Authorization.Requirements;

/// <summary>
/// Требование авторизации на основе ролей пользователя.
/// </summary>
internal sealed class RoleBasedAuthenticationRequirement : IAuthorizationRequirement
{
    public HashSet<string> Roles { get; }

    /// <summary>
    /// Создает новый экземпляр требования авторизации на основе ролей пользователя.
    /// </summary>
    /// <param name="roles">Список ролей пользователя, необходимых для успешной авторизации.</param>
    public RoleBasedAuthenticationRequirement(IEnumerable<Roles> roles) =>
        Roles = roles.Select(role => role.ToString()).ToHashSet();
}