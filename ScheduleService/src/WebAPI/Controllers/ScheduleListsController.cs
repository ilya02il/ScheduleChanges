using Application.ScheduleLists.Commands;
using Application.ScheduleLists.Queries;
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
    [Route("schedule-lists")]
    public class ScheduleListsController : ControllerBase
    {
        private readonly ISender _sender;

        public ScheduleListsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetScheduleListsByGroupId([FromQuery] Guid groupId,
            CancellationToken cancellationToken)
        {
            var query = new GetScheduleListsByGroupIdQuery(groupId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetScheduleListsById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetScheduleListByIdQuery(id);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleList([FromQuery] Guid groupId,
            CreateScheduleListCommand command,
            CancellationToken cancellationToken)
        {
            command.GroupId = groupId;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest("The list wasn't created (list id: {id}).");

            return Ok($"The list was successfully created.");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateScheduleList([FromRoute] Guid id,
            UpdateScheduleListCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest($"The list wasn't updated (list id: {id}).");

            return Ok($"The list was successfully updated (list id: {id}).");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteScheduleList([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteScheduleListCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest($"The list wasn't deleted (list id: {id}).");

            return Ok($"The list was successfully updated (list id: {id}).");
        }
    }
}
