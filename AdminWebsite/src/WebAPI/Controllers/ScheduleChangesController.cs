using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("schedule-changes")]
    public class ScheduleChangesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChangesListById([FromRoute] long id,
            CancellationToken cancellationToken)
        {
            try
            {
                //var result = await _mediator.Send(new GetChangesListByIdQuery(listId), cancellationToken);

                //return Ok(result);

                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetChangesListItems([FromRoute] long id,
            CancellationToken cancellationToken)
        {

            return await Task.FromResult(Ok());
        }

        [HttpPost("from-file")]
        public async Task<IActionResult> CreateListFromFile([FromBody] DateTime date,
            [FromBody] IFormFile file,
            //[FromBody] ChangesTableMapDto tableMap,
            CancellationToken cancellationToken)
        {
            //var result = await _mediator.Send(new CreateChangesListFromFileCommand { Date = date, File = file, TableMap = tableMap }, cancellationToken);

            //return Created(nameof(CreateListFromFile), result);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChangesList([FromRoute] long id,
            [FromBody] object command,
            CancellationToken cancellationToken)
        {

            return await Task.FromResult(Ok());
        }
    }
}
