using Application.CallSchedules.Commands;
using Application.CallSchedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("call-schedule-lists")]
    public class CallScheduleListsController : ControllerBase
    {
        private readonly ISender _sender;

        public CallScheduleListsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetCallScheduleListByEducOrgIdAndDayOfWeek([FromQuery] Guid educOrgId,
            [FromQuery] DayOfWeek dayOfWeek,
            CancellationToken cancellationToken)
        {
            var senderRequest = new GetCallScheduleListQuery(educOrgId, dayOfWeek);

            var senderResponse = await _sender.Send(senderRequest, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateCallScheduleListItem(CreateCallScheduleListItemCommand command,
            CancellationToken cancellationToken)
        {
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpPost("items/{id:guid}")]
        public async Task<IActionResult> UpdateCallScheduleListItem([FromRoute] Guid id,
            UpdateCallScheduleListItemCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpDelete("items/{id:guid}")]
        public async Task<IActionResult> DeleteCallScheduleListItem([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteCallScheduleListItemCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest(senderResponse);

            return Ok(senderResponse);
        }
    }
}
