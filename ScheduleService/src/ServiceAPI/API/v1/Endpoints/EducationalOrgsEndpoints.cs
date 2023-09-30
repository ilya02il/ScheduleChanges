using Application.EducOrgs.Commands;
using Application.EducOrgs.Dtos;
using Application.EducOrgs.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.ServiceAPI.Enums;
using ScheduleService.ServiceAPI.Extensions;

namespace ScheduleService.ServiceAPI.API.v1.Endpoints;

/// <summary>
/// Конфигурация конечных точек API для работы с образовательными организациями.
/// </summary>
internal static class EducationalOrgsEndpoints
{
    /// <summary>
    /// Сконфигурировать конечные точки API для работы с образовательными организациями.
    /// </summary>
    /// <param name="apiRouteGroupBuilder"></param>
    public static void MapEducationalOrgsEndpoints(this RouteGroupBuilder apiRouteGroupBuilder)
    {
        var educationalOrgsRouteGroupBuilder = apiRouteGroupBuilder
            .MapGroup("/educational-orgs")
            .WithTags("EducationalOrgs")
            .RequireRoleBasedAuthorization(Roles.Admin)
            .RequireJwtSourceAuthorization();

        educationalOrgsRouteGroupBuilder
            .MapGet("/{id:guid}", GetEducationalOrgById)
            .WithName(nameof(GetEducationalOrgById));

        educationalOrgsRouteGroupBuilder
            .MapGet("/brief", GetBriefEducationalOrg)
            .WithName(nameof(GetBriefEducationalOrg))
            .AllowAnonymous();

        educationalOrgsRouteGroupBuilder
            .MapPost("/", CreateEducationalOrg)
            .WithName(nameof(CreateEducationalOrg));

        educationalOrgsRouteGroupBuilder
            .MapPut("/{id:guid}", UpdateEducationalOrg)
            .WithName(nameof(UpdateEducationalOrg));

        educationalOrgsRouteGroupBuilder
            .MapDelete("/{id:guid}", DeleteEducationalOrg)
            .WithName(nameof(DeleteEducationalOrg));
    }

    /// <summary>
    /// Получить образовательную организацию по ее ИД.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Модель с информацией об образовательной организации.</returns>
    private static async Task<Ok<EducOrgDto>> GetEducationalOrgById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetEducOrgByIdQuery(id);
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Получить словарь с ИД образовательных организаций и их названиями.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Словарь с ИД образовательных организаций и их названиями.</returns>
    private static async Task<Ok<IEnumerable<BriefEducOrgDto>>> GetBriefEducationalOrg(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetBriefEducOrgsQuery();
        var senderResponse = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(senderResponse);
    }

    /// <summary>
    /// Создать образовательную организацию.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="command">Модель с информацией, необходимой для создания образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>ИД созданной образовательной организации.</returns>
    private static async Task<Results<CreatedAtRoute<Guid>, BadRequest<string>>> CreateEducationalOrg(
        [FromServices] ISender sender,
        [FromBody] CreateEducOrgCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var senderResponse = await sender.Send(command, cancellationToken);

            return TypedResults.CreatedAtRoute(
                senderResponse,
                nameof(GetEducationalOrgById),
                new { Id = senderResponse });
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить информацию об образовательной организации.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор обновляемой образовательной организации.</param>
    /// <param name="command">Модель с информацией, необходимой для обновления образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> UpdateEducationalOrg(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateEducOrgCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
            return TypedResults
                .BadRequest("При обновлении информации об образовательной организации произошла ошибка.");

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить образовательную организацию.
    /// </summary>
    /// <param name="sender">Диспетчер обработчикав команд и запросов.</param>
    /// <param name="id">Идентификатор удаляемой образовательной организации.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    private static async Task<Results<NoContent, BadRequest<string>>> DeleteEducationalOrg(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteEducOrgCommand(id);
        var senderResponse = await sender.Send(command, cancellationToken);

        if (!senderResponse)
            return TypedResults
                .BadRequest("При удалении образовательной организации произошла ошибка.");

        return TypedResults.NoContent();
    }
}