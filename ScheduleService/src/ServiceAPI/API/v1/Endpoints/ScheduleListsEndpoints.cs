using Application.ScheduleLists.Commands;
using Application.ScheduleLists.Dtos;
using Application.ScheduleLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.ServiceAPI.Enums;
using ScheduleService.ServiceAPI.Extensions;

namespace ScheduleService.ServiceAPI.API.v1.Endpoints;

/// <summary>
/// Конфигурация кончных точек API для работы с расписанием занятий.
/// </summary>
internal static class ScheduleListsEndpoints
{
    /// <summary>
    /// Сконфигурировать конечные точки API для работы с расписанием занятий.
    /// </summary>
    /// <param name="apiRouteGroupBuilder">Билдер группы конечных точек API.</param>
    public static void MapScheduleListsEndpoints(this RouteGroupBuilder apiRouteGroupBuilder)
    {
        var scheduleListsEndpointsGroup = apiRouteGroupBuilder
            .MapGroup("/schedule-lists")
            .WithTags("ScheduleLists")
            .RequireJwtSourceAuthorization()
            .RequireRoleBasedAuthorization(Roles.Admin, Roles.EducOrgManager);

        scheduleListsEndpointsGroup
            .MapGet("/", GetScheduleListByGroupId)
            .WithName(nameof(GetScheduleListByGroupId))
            .AllowAnonymous();

        scheduleListsEndpointsGroup
            .MapGet("/{id:guid}", GetScheduleListById)
            .WithName(nameof(GetScheduleListById));

        scheduleListsEndpointsGroup
            .MapPost("/", CreateScheduleList)
            .WithName(nameof(CreateScheduleList));

        scheduleListsEndpointsGroup
            .MapPut("/{id:guid}", UpdateScheduleList)
            .WithName(nameof(UpdateScheduleList));

        scheduleListsEndpointsGroup
            .MapDelete("/{id:guid}", DeleteScheduleList)
            .WithName(nameof(DeleteScheduleList));
    }

    /// <summary>
    /// Получить список с расписаниями занятий по идентификатору группы.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список с расписаниями занятий в соответствии с идентификатором группы.</returns>
    private static async Task<Ok<IEnumerable<ScheduleListDto>>> GetScheduleListByGroupId(
        [FromServices] ISender sender,
        [FromQuery] Guid groupId,
        CancellationToken cancellationToken)
    {
        var query = new GetScheduleListsByGroupIdQuery(groupId);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Получить расписание занятий по идентификатору.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор расписания занятий.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Расписание занятий в соответствии с его идентификатором.</returns>
    private static async Task<Ok<ScheduleListDto>> GetScheduleListById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetScheduleListByIdQuery(id);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Создать расписание занятий.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="groupId">Идентификатор группы, для которой создается расписание занятий.</param>
    /// <param name="command">Модель с информацией, необходимой для создания расписания занятий.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Идентификатор созданного расписания занятий.</returns>
    private static async Task<Results<Created, BadRequest<string>>> CreateScheduleList(
        [FromServices] ISender sender,
        [FromQuery] Guid groupId,
        [FromBody] CreateScheduleListCommand command,
        CancellationToken cancellationToken)
    {
        command.GroupId = groupId;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При создании расписания произошла ошибка.");
        }

        return TypedResults.Created(string.Empty);
    }

    /// <summary>
    /// Обновить расписание занятий.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемого расписания занятий.</param>
    /// <param name="command">Модель с информацией, необходимой для обновления расписания занятий.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateScheduleList(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateScheduleListCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При обновлении информации о расписании произошла ошибка.");
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить расписание занятий.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемого расписания занятий.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteScheduleList(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteScheduleListCommand(id);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При удалении расписания произошла ошибка.");
        }

        return TypedResults.NoContent();
    } 
}