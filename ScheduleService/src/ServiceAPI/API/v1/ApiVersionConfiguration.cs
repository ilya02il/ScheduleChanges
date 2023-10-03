using Asp.Versioning;
using ScheduleService.ServiceAPI.API.v1.Endpoints;

namespace ScheduleService.ServiceAPI.API.v1;

/// <summary>
/// Конфигурация API версии 1.x.x.
/// </summary>
internal static class ApiVersionConfiguration
{
    /// <summary>
    /// Сконфигурировать конечные точки API.
    /// </summary>
    /// <param name="endpointRouteBuilder">Билдер конечных точек API.</param>
    public static void MapApiV1(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var apiGroup = endpointRouteBuilder
            .MapGroup("/")
            .HasApiVersion(new ApiVersion(1, 0));

        apiGroup.MapCallScheduleListsEndpoints();
        apiGroup.MapEducationalOrgsEndpoints();
        apiGroup.MapGroupsEndpoints();
        apiGroup.MapScheduleChangesListsEndpoints();
        apiGroup.MapScheduleListsEndpoints();
    }
}