using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.ScheduleLists;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("schedule-lists")]
    public class ScheduleListsController : ControllerBase
    {
        private readonly GrpcScheduleListsClientService _scheduleListsService;

        public ScheduleListsController(GrpcScheduleListsClientService scheduleListsService)
        {
            _scheduleListsService = scheduleListsService;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetScheduleListsByGroupId([FromQuery] Guid groupId,
            CancellationToken cancellationToken)
        {
            var result = await _scheduleListsService.GetScheduleListsByGroupId(groupId,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetScheduleListsById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _scheduleListsService.GetScheduleListById(id,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleList([FromQuery] Guid groupId,
            CreateScheduleListDto scheduleList,
            CancellationToken cancellationToken)
        {
            var result = await _scheduleListsService.CreateScheduleList(
                groupId,
                scheduleList,
                cancellationToken);

            if (!result)
                return BadRequest($"The list wasn't created.");

            return Ok($"The list was successfully created.");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateScheduleList([FromRoute] Guid id,
            UpdateScheduleListDto listItem,
            CancellationToken cancellationToken)
        {
            var result = await _scheduleListsService.UpdateScheduleList(id,
                listItem,
                cancellationToken);

            if (!result)
                return BadRequest($"The list wasn't updated (list id: {id}).");

            return Ok($"The list was successfully updated (list id: {id}).");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteScheduleList([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _scheduleListsService.DeleteScheduleList(id,
                cancellationToken);

            if (!result)
                return BadRequest($"The list wasn't deleted (list id: {id}).");

            return Ok($"The list was successfully updated (list id: {id}).");
        }
    }
}
