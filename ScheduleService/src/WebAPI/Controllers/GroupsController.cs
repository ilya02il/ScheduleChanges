using Application.Groups.Commands;
using Application.Groups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("groups")]
    public class GroupsController : ControllerBase
    {
        private readonly ISender _sender;

        public GroupsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsListByEducOrgId([FromQuery] Guid educOrgId,//delete this when the identity service will be created
            CancellationToken cancellationToken)
        {
            var query = new GetGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("brief")]
        public async Task<IActionResult> GetBriefGroupsByEducOrgId([FromQuery] Guid educOrgId,//delete this when the identity service will be created
            CancellationToken cancellationToken)
        {
            var query = new GetBriefGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromQuery] Guid educOrdId,//delete this when the identity service will be created
            [FromBody] CreateGroupCommand command,
            CancellationToken cancellationToken)
        {
            command.EducOrgId = educOrdId;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGroupInfo([FromRoute]Guid id,
            UpdateGroupCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGroup([FromRoute]Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteGroupCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }
    }
}
