using Application.EducOrgs.Commands;
using Application.EducOrgs.Queries;
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
    [Route(ApiBaseRoute.BaseRoute + "/educational-orgs")]
    [AuthorizeOnJwtSource(Roles = "Admin")]
    public class EducOrgsController : ControllerBase
    {
        private readonly ISender _sender;

        public EducOrgsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetEducOrgByIdQuery(id);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefEducOrgs(CancellationToken cancellationToken)
        {
            var query = new GetBriefEducOrgsQuery();
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEducOrg([FromBody] CreateEducOrgCommand command, 
            CancellationToken cancellationToken)
        {
            try
            {
                var senderResponse = await _sender.Send(command, cancellationToken);

                return Created(string.Empty, senderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEducOrg([FromRoute] Guid id,
            [FromBody] UpdateEducOrgCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEducOrg([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteEducOrgCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return NoContent();
        }
    }
}
