using Application.ChangesLists.Commands.CreateChangesList;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Application.ChangesLists.Commands.DeleteChangesList;
using Application.ChangesLists.Commands.DeleteChangesListItem;
using Application.ChangesLists.Commands.UpdateChangesList;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using Application.ChangesLists.Queries.GetBriefScheduleChangesList;
using Application.ChangesLists.Queries.GetScheduleChangesList;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.ServiceAPI.Enums;
using ScheduleService.ServiceAPI.Extensions;
using ScheduleService.ServiceAPI.Helpers;

namespace ScheduleService.ServiceAPI.API.v1.Endpoints;

/// <summary>
/// Конфигурация кончных точек API для работы со списком изменений в расписании.
/// </summary>
internal static class ScheduleChangesListsEndpoints
{
    private const string EducOrgIdClaimType = "educ_org_id";
    
    /// <summary>
    /// Сконфигурировать конечные точки API для работы с изменениями в расписании.
    /// </summary>
    /// <param name="apiRouteGroupBuilder">Билдер группы конечных точек API.</param>
    public static void MapScheduleChangesListsEndpoints(this RouteGroupBuilder apiRouteGroupBuilder)
    {
        var scheduleListsEndpointsGroup = apiRouteGroupBuilder
            .MapGroup("/schedule-changes-lists")
            .WithTags("ScheduleChangesLists")
            .RequireJwtSourceAuthorization()
            .RequireRoleBasedAuthorization(Roles.Admin, Roles.EducOrgManager);

        scheduleListsEndpointsGroup
            .MapGet("/{id:guid}", GetChangesListById)
            .WithName(nameof(GetChangesListById))
            .AllowAnonymous();

        scheduleListsEndpointsGroup
            .MapGet("/brief", GetBriefChangesListsByEducOrgId)
            .WithName(nameof(GetBriefChangesListsByEducOrgId));

        scheduleListsEndpointsGroup
            .MapPost("/", CreateChangesList)
            .WithName(nameof(CreateChangesList));

        scheduleListsEndpointsGroup
            .MapPut("/{id:guid}", UpdateChangesList)
            .WithName(nameof(UpdateChangesList));

        scheduleListsEndpointsGroup
            .MapDelete("/{id:guid}", DeleteChangesList)
            .WithName(nameof(DeleteChangesList));

        scheduleListsEndpointsGroup
            .MapPost("/{listId:guid}/items", CreateChangesListItem)
            .WithName(nameof(CreateChangesListItem));

        scheduleListsEndpointsGroup
            .MapPut("/items/{id:guid}", UpdateChangesListItem)
            .WithName(nameof(UpdateChangesListItem));

        scheduleListsEndpointsGroup
            .MapDelete("/items/{id:guid}", DeleteChangesListItem)
            .WithName(nameof(DeleteChangesListItem));
    }

    /// <summary>
    /// Получить список изменений в расписании по его идентификатору.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор списка изменений в расписании.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список изменений в расписании в соответствии с его идентификатором.</returns>
    private static async Task<Ok<ChangesListEntity>> GetChangesListById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetChangesListByIdQuery(id);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Получить словарь с идентификаторами списков изменений в расписании и их датами по
    /// идентификатору образовательной организации пользователя.
    /// </summary>
    /// <param name="context">HTTP-контекст запроса.</param>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>
    /// Словарь с идентификаторами списков изменений в расписании и их датами по
    /// идентификатору образовательной организации пользователя.
    /// </returns>
    private static async Task<Results<
        Ok<IEnumerable<BriefChangesListDto>>,
        BadRequest<string>>
    > GetBriefChangesListsByEducOrgId(
        HttpContext context,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var educOrgIdClaim = ClaimsHelper
            .GetClaimValueFromCurrentUserClaims(context.User, EducOrgIdClaimType);
        
        var isParsed = Guid.TryParse(educOrgIdClaim, out var educOrgId);

        if (!isParsed)
        {
            return TypedResults
                .BadRequest("Данные пользователя содержат некорректный идентификатор образовательной организации.");
        }

        var query = new GetBriefScheduleChangesListsQuery(educOrgId);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Создать список изменений в расписании.
    /// </summary>
    /// <param name="context">HTTP-контекст запроса.</param>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="command">Модель с информацией, необходимой для создания списка изменений в расписании.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Идентификатор созданного списка изменений в расписании.</returns>
    private static async Task<Results<Created, BadRequest<string>>> CreateChangesList(
        HttpContext context,
        [FromServices] ISender sender,
        [FromBody] CreateChangesListCommand command,
        CancellationToken cancellationToken)
    {
        var educOrgIdClaims = ClaimsHelper
            .GetClaimValueFromCurrentUserClaims(context.User, EducOrgIdClaimType);
        
        command.EducOrgId = Guid.Parse(educOrgIdClaims);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults.BadRequest("При создании списка изменений в расписании произошла ошибка.");
        }

        return TypedResults.Created(string.Empty);
    }

    /// <summary>
    /// Обновить список изменений в расписании.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемого списка.</param>
    /// <param name="command">Модель с информацией, необходимой для обновления списка изменений в расписании.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateChangesList(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateChangesListCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При обновлении списка изменений в расписании произошла ошибка.");
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить список изменений в расписании.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемого списка изменений в расписании.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteChangesList(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteChangesListCommand(id);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults.BadRequest("При удалении списка изменений в расписании произошла ошибка.");
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Создать позицию списка изменений в расписании.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="listId">Идентификатор списка изменений в расписании, в котором необходимо создать позицию.</param>
    /// <param name="command">
    /// Модель с информацией, необходимой для создания позиции списка изменений в расписании.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<Created, BadRequest<string>>> CreateChangesListItem(
        [FromServices] ISender sender,
        [FromRoute] Guid listId,
        [FromBody] CreateChangesListItemCommand command,
        CancellationToken cancellationToken)
    {
        command.ListId = listId;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При добавлении информации об изменении в расписании произошла ошибка.");
        }

        return TypedResults.Created(string.Empty);
    }

    /// <summary>
    /// Обновить позицию списка изменений в расписании.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемой позиции списка изменений в расписании.</param>
    /// <param name="command">
    /// Модель с информацией, необходимой для обновления позиции списка изменений в расписании.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateChangesListItem(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateChangesListItemCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При обновлении информации об изменении в расписании произошла ошибка.");
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить позицию списка изменений в расписании.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемой позиции списка изменений в расписании.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteChangesListItem(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteChangesListItemCommand(id);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
        {
            return TypedResults
                .BadRequest("При удалении информации об изменении в расписании произошла ошибка.");            
        }

        return TypedResults.NoContent();
    }
}