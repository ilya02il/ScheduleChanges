﻿using Application.Groups.Commands;
using Application.Groups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Attributes;
using ServiceAPI.Helpers;
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
        private const string EducOrgIdClaimType = "educ_org_id";

        private readonly ISender _sender;

        public GroupsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsListByEducOrgId(CancellationToken cancellationToken)
        {
            Guid.TryParse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType), out Guid educOrgId);

            var query = new GetGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefGroupsByEducOrgId([FromQuery] Guid educOrgId,
            CancellationToken cancellationToken)
        {
            bool isParsed = Guid.TryParse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType),
                out Guid eoId);

            if (isParsed)
                educOrgId = eoId;

            var query = new GetBriefGroupsByEducOrgIdQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupCommand command,
            CancellationToken cancellationToken)
        {
            command.EducOrgId = Guid.Parse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType));
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
