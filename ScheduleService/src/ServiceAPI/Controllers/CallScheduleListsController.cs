using Application.CallSchedules.Commands;
using Application.CallSchedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Attributes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI.Controllers
{
    [ApiController]
    [AuthorizeOnJwtSource(Roles = "Admin, EducOrgManager")]
    [Route(ApiBaseRoute.BaseRoute + "/call-schedule-lists")]
    public class CallScheduleListsController : ControllerBase
    {
        private readonly ISender _sender;

        public CallScheduleListsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCallScheduleListByEducOrgIdAndDayOfWeek([FromQuery] Guid educOrgId,
            [FromQuery] DayOfWeek dayOfWeek,
            CancellationToken cancellationToken)
        {
            var senderRequest = new GetCallScheduleListQuery(educOrgId, dayOfWeek);
            var senderResponse = await _sender.Send(senderRequest, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateCallScheduleListItem([FromQuery] Guid educOrgId,
            [FromBody] CreateCallScheduleListItemCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                command.EducOrgId = educOrgId;
                var senderResponse = await _sender.Send(command, cancellationToken);

                return Created(string.Empty, senderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("items/{id:guid}")]
        public async Task<IActionResult> UpdateCallScheduleListItem([FromRoute] Guid id,
            UpdateCallScheduleListItemCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                command.Id = id;
                var senderResponse = await _sender.Send(command, cancellationToken);

                if (!senderResponse)
                    return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("items/{id:guid}")]
        public async Task<IActionResult> DeleteCallScheduleListItem([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteCallScheduleListItemCommand(id);
                var senderResponse = await _sender.Send(command, cancellationToken);

                if (!senderResponse)
                    return BadRequest(senderResponse);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
