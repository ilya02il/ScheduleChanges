using ScheduleService.ServiceAPI.Authorization.Requirements;
using ScheduleService.ServiceAPI.Enums;

namespace ScheduleService.ServiceAPI.Extensions;

/// <summary>
/// Класс с методами расширения билдера соглашений для конеченых точек API.
/// </summary>
internal static class EndpointConventionBuilderExtensions
{
    /// <summary>
    /// Требовать авторизацию с использованием JWT, проверяемого сервисом идентификации.
    /// </summary>
    /// <param name="endpointConventionBuilder">Билдер соглашений для конечных точек API.</param>
    /// <typeparam name="TEndpointConventionBuilder">Тип билдера соглашений для конечных точек API.</typeparam>
    /// <returns>Билдер соглашений для конечных точек API.</returns>
    public static TEndpointConventionBuilder RequireJwtSourceAuthorization<TEndpointConventionBuilder>(
        this TEndpointConventionBuilder endpointConventionBuilder
    ) where TEndpointConventionBuilder : IEndpointConventionBuilder
    {
        return endpointConventionBuilder
            .RequireAuthorization(policyBuilder =>
                policyBuilder.Requirements.Add(new JwtSourceAuthorizationRequirement())
            );
    }

    /// <summary>
    /// Требовать авторизацию на основе ролей.
    /// </summary>
    /// <param name="endpointConventionBuilder">Билдер соглашений для конечных точек API.</param>
    /// <param name="roles">Список ролей, которым разрешен доступ.</param>
    /// <typeparam name="TEndpointConventionBuilder">Тип билдера соглашений для конечных точек API.</typeparam>
    /// <returns>Билдер соглашений для конечных точек API.</returns>
    public static TEndpointConventionBuilder RequireRoleBasedAuthorization<TEndpointConventionBuilder>(
        this TEndpointConventionBuilder endpointConventionBuilder,
        params Roles[] roles
    ) where TEndpointConventionBuilder : IEndpointConventionBuilder
    {
        return endpointConventionBuilder
            .RequireAuthorization(policyBuilder =>
                policyBuilder.Requirements.Add(new RoleBasedAuthenticationRequirement(roles))
            );
    }
}