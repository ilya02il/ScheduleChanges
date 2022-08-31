using Application.Groups.Commands;
using Application.Groups.Queries;
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
    [Route(ApiBaseRoute.BaseRoute + "/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly ISender _sender;

        public GroupsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetGroupByIdQuery(id);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsListByEducOrgId([FromQuery] Guid educOrgId,
            CancellationToken cancellationToken)
        {
            var query = new GetGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefGroupsByEducOrgId([FromQuery] Guid educOrgId,
            CancellationToken cancellationToken)
        {
            var query = new GetBriefGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var senderResponse = await _sender.Send(command, cancellationToken);
                return Created(ApiBaseRoute.BaseRoute + $"/groups/{senderResponse}", senderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGroup([FromRoute]Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteGroupCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return NoContent();
        }
    }
}
