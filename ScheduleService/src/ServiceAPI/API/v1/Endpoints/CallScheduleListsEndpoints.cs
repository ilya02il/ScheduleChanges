using Application.CallSchedules.Commands;
using Application.CallSchedules.Dtos;
using Application.CallSchedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.ServiceAPI.Enums;
using ScheduleService.ServiceAPI.Extensions;

namespace ScheduleService.ServiceAPI.API.v1.Endpoints;

/// <summary>
/// Конфигурация кончных точек API для работы с расписанием звонков.
/// </summary>
internal static class CallScheduleListsEndpoints
{
    /// <summary>
    /// Сконфигурировать конечные точки API для работы с расписанием звонков.
    /// </summary>
    /// <param name="apiRouteGroupBuilder">Билдер группы конечных точек API.</param>
    public static void MapCallScheduleListsEndpoints(this RouteGroupBuilder apiRouteGroupBuilder)
    {
        var callScheduleListsRouteGroupBuilder = apiRouteGroupBuilder
            .MapGroup("/call-schedule-lists")
            .WithTags("CallScheduleLists")
            .RequireRoleBasedAuthorization(Roles.Admin, Roles.EducOrgManager)
            .RequireJwtSourceAuthorization();

        callScheduleListsRouteGroupBuilder
            .MapGet("/", GetCallScheduleListByEducOrgIdAndDayOfWeek)
            .WithName(nameof(GetCallScheduleListByEducOrgIdAndDayOfWeek))
            .AllowAnonymous();

        callScheduleListsRouteGroupBuilder
            .MapPost("/items", CreateCallScheduleListItem)
            .WithName(nameof(CreateCallScheduleListItem));

        callScheduleListsRouteGroupBuilder
            .MapPut("/items/{id:guid}", UpdateCallScheduleListItem)
            .WithName(nameof(UpdateCallScheduleListItem));

        callScheduleListsRouteGroupBuilder
            .MapDelete("/items/{id:guid}", DeleteCallScheduleListItem)
            .WithName(nameof(DeleteCallScheduleListItem));
    }

    /// <summary>
    /// Получить расписание звонков по ИД образовательной организации и дню недели.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="educOrgId">Идентификатор образовательной организации.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Расписание звонков в соответствии с ИД образовательной организации и днем недели.</returns>
    private static async Task<Ok<CallScheduleListDto>> GetCallScheduleListByEducOrgIdAndDayOfWeek(
        [FromServices] ISender sender,
        [FromQuery] Guid educOrgId,
        [FromQuery] DayOfWeek dayOfWeek,
        CancellationToken cancellationToken)
    {
        var senderRequest = new GetCallScheduleListQuery(educOrgId, dayOfWeek);
        var senderResponse = await sender.Send(senderRequest, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }
    
    /// <summary>
    /// Создать расписание звонков.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="educOrgId">Идентификатор образовательной организации.</param>
    /// <param name="command">Модель с информацией, необходимой для создания расписания звонков.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Идентификатор созданного расписания звонков.</returns>
    private static async Task<Results<Created<Guid>, BadRequest<string>>> CreateCallScheduleListItem(
        [FromServices] ISender sender,
        [FromQuery] Guid educOrgId,
        [FromBody] CreateCallScheduleListItemCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            command.EducOrgId = educOrgId;
            var senderResponse = await sender.Send(command, cancellationToken);

            return TypedResults.Created(string.Empty, senderResponse);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить информацию о расписании звонков.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемого расписания звонков.</param>
    /// <param name="command">Модель с информацией, необходимой для обновления расписания звонков.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateCallScheduleListItem(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateCallScheduleListItemCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            command.Id = id;
            var senderResponse = await sender.Send(command, cancellationToken);

            if (!senderResponse)
            {
                return TypedResults
                    .BadRequest("При обновлении позиции расписания произошла ошибка.");
            }

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить расписание звонков.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемого расписания звонков.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteCallScheduleListItem(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteCallScheduleListItemCommand(id);
            var senderResponse = await sender.Send(command, cancellationToken);

            if (!senderResponse)
            {
                return TypedResults
                    .BadRequest("При удалении позиции расписания произошла ошибка.");
            }

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}