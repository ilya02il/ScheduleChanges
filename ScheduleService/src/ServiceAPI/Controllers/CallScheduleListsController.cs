using Application.CallSchedules.Commands;
using Application.CallSchedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Attributes;
using ServiceAPI.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI.Controllers
{
    [ApiController]
    [AuthorizeOnJwtSource]
    [Route(ApiBaseRoute.BaseRoute + "/call-schedule-lists")]
    public class CallScheduleListsController : ControllerBase
    {
        private const string EducOrgIdClaimType = "educ_org_id";

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
        public async Task<IActionResult> CreateCallScheduleListItem(CreateCallScheduleListItemCommand command,
            CancellationToken cancellationToken)
        {
            command.EducOrgId = Guid.Parse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType));
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpPut("items/{id:guid}")]
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
