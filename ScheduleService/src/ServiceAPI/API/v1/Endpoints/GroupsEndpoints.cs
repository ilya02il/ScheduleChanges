using Application.Groups.Commands;
using Application.Groups.Dtos;
using Application.Groups.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.ServiceAPI.Enums;
using ScheduleService.ServiceAPI.Extensions;

namespace ScheduleService.ServiceAPI.API.v1.Endpoints;

/// <summary>
/// Конфигурация конечных точек API для работы с группами.
/// </summary>
internal static class GroupsEndpoints
{
    /// <summary>
    /// Сконфигурировать конечные точки API для работы с группами.
    /// </summary>
    /// <param name="apiRouteGroupBuilder">Билдер группы конечных точек API.</param>
    public static void MapGroupsEndpoints(this RouteGroupBuilder apiRouteGroupBuilder)
    {
        var groupsEndpointRouteGroupBuilder = apiRouteGroupBuilder
            .MapGroup("/groups")
            .WithTags("Groups")
            .RequireRoleBasedAuthorization(Roles.Admin, Roles.EducOrgManager)
            .RequireJwtSourceAuthorization();

        groupsEndpointRouteGroupBuilder
            .MapGet("/{id:guid}", GetGroupById)
            .WithName(nameof(GetGroupById));

        groupsEndpointRouteGroupBuilder
            .MapGet("/", GetGroupsListByEducOrgId)
            .WithName(nameof(GetGroupsListByEducOrgId));

        groupsEndpointRouteGroupBuilder
            .MapGet("/brief", GetBriefGroupsListByEducOrgId)
            .WithName(nameof(GetBriefGroupsListByEducOrgId))
            .AllowAnonymous();

        groupsEndpointRouteGroupBuilder
            .MapPost("/", CreateGroup)
            .WithName(nameof(CreateGroup));

        groupsEndpointRouteGroupBuilder
            .MapPut("/{id:guid}", UpdateGroup)
            .WithName(nameof(UpdateGroup));

        groupsEndpointRouteGroupBuilder
            .MapDelete("/{id:guid}", DeleteGroup)
            .WithName(nameof(DeleteGroup));
    }

    /// <summary>
    /// Получить группу по ее идентификатору.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор группы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Модель с информацией о группе.</returns>
    private static async Task<Ok<GroupDto>> GetGroupById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetGroupByIdQuery(id);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Получить список групп по идентификатору образовательной организации.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="educOrgId">Идентификатор образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список групп в соответствии с идентификатором образовательной организации.</returns>
    private static async Task<Ok<IEnumerable<GroupDto>>> GetGroupsListByEducOrgId(
        [FromServices] ISender sender,
        [FromQuery] Guid educOrgId,
        CancellationToken cancellationToken)
    {
        var query = new GetGroupsByEducOrgIdQuery(educOrgId);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }
    
    /// <summary>
    /// Получить словарь с идентификаторами групп и их названиями в
    /// соответствии с идентификатором образовательной организации.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="educOrgId">Идентификатор образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>
    /// Словарь с идентификаторами групп и их названиями в
    /// соответствии с идентификатором образовательной организации.
    /// </returns>
    private static async Task<Ok<IEnumerable<BriefGroupDto>>> GetBriefGroupsListByEducOrgId(
        [FromServices] ISender sender,
        [FromQuery] Guid educOrgId,
        CancellationToken cancellationToken)
    {
        var query = new GetBriefGroupsByEducOrgIdQuery(educOrgId);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }
    
    /// <summary>
    /// Создать группу.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="command">Модель с информацией, необходимой для создания группы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Идентификатор созданной группы.</returns>
    private static async Task<Results<CreatedAtRoute<Guid>, BadRequest<string>>> CreateGroup(
        [FromServices] ISender sender,
        [FromBody] CreateGroupCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var senderResponse = await sender.Send(command, cancellationToken);
            return TypedResults.CreatedAtRoute(
                senderResponse,
                nameof(GetGroupById),
                new { Id = senderResponse });
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить информацию о группе.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемой группы.</param>
    /// <param name="command">Модель с информацией, необходимой для обновления группы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateGroup(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateGroupCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При обновлении информации о группе произошла ошибка.");
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить группу.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемой группы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteGroup(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteGroupCommand(id);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
            return TypedResults
                .BadRequest("При удалении группы произошла ошибка.");

        return TypedResults.NoContent();
    }
}