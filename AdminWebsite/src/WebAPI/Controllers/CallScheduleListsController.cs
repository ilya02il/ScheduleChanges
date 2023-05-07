using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.CallScheduleLists;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("call-schedule-lists")]
    public class CallScheduleListsController : ControllerBase
    {
        private readonly GrpcCallScheduleListsClientService _callSchedulesService;

        public CallScheduleListsController(GrpcCallScheduleListsClientService callSchedulesService)
        {
            _callSchedulesService = callSchedulesService;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetCallScheduleListByEducOrgIdAndDayOfWeek([FromQuery] Guid educOrgId,
            [FromQuery] DayOfWeek dayOfWeek,
            CancellationToken cancellationToken)
        {
            var result = await _callSchedulesService.GetCallScheduleList(educOrgId,
                dayOfWeek,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateCallScheduleListItem([FromQuery] Guid educOrgId,
            CreateCallScheduleListItemDto listItem,
            CancellationToken cancellationToken)
        {
            var result = await _callSchedulesService.CreateCallScheduleListItem(educOrgId,
                listItem,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("items/{id:guid}")]
        public async Task<IActionResult> UpdateCallScheduleListItem([FromRoute] Guid id,
            UpdateCallScheduleListItemDto listItem,
            CancellationToken cancellationToken)
        {
            var result = await _callSchedulesService.UpdateCallScheduleListItem(id,
                listItem,
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("items/{id:guid}")]
        public async Task<IActionResult> DeleteCallScheduleListItem([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _callSchedulesService.DeleteCallScheduleListItem(id,
                cancellationToken);

            return Ok(result);
        }
    }
}
