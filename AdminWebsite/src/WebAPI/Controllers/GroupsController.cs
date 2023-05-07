using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.Groups;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("groups")]
    public class GroupsController : ControllerBase
    {
        private readonly GrpcGroupsClientService _groupsService;

        public GroupsController(GrpcGroupsClientService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsListByEducOrgId([FromQuery]Guid educOrgId,//delete this when the identity service will be created
            CancellationToken cancellationToken)
        {
            var result = await _groupsService.GetGroupListByEducOrgId(educOrgId,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("brief")]
        public async Task<IActionResult> GetBriefGroupsByEducOrgId([FromQuery]Guid educOrgId,//delete this when the identity service will be created
            CancellationToken cancellationToken)
        {
            var result = await _groupsService.GetBriefGroupsByEducOrgId(educOrgId,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromQuery] Guid educOrdId,//delete this when the identity service will be created
            [FromBody]CreateGroupDto group,
            CancellationToken cancellationToken)
        {
            var result = await _groupsService.CreateGroup(educOrdId,
                group,
                cancellationToken);

            if (!result)
                return BadRequest("The group wasn't created.");

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGroupInfo([FromRoute]Guid id,
            GroupInfoDto groupInfo,
            CancellationToken cancellationToken)
        {
            var result = await _groupsService.UpdateGroupInfo(id,
                groupInfo,
                cancellationToken);

            if (!result)
                return BadRequest("The group info wasn't updated.");

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGroup([FromRoute]Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _groupsService.DeleteGroup(id, cancellationToken);

            if (!result)
                return BadRequest("The group wasn't deleted.");

            return Ok(result);
        }
    }
}
