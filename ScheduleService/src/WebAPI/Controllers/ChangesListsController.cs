using Application.ChangesLists.Commands;
using Application.ChangesLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("schedule-service/changes-lists")]
    public class ChangesListsController : ControllerBase
    {
        private readonly ISender _mediator;

        public ChangesListsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChangesListById([FromRoute(Name = "id")] Guid listId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new GetChangesListByIdQuery(listId), cancellationToken);

                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetChangesListItems([FromRoute(Name = "id")] Guid listId,
            CancellationToken cancellationToken)
        {

            return await Task.FromResult(Ok());
        }

        [HttpPost("from-file")]
        public async Task<IActionResult> CreateListFromFile([FromBody] DateTime date,
            [FromBody] IFormFile file,
            [FromBody] ChangesTableMapDto tableMap,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateChangesListFromFileCommand { Date = date, File = file, TableMap = tableMap }, cancellationToken);

            return Created(nameof(CreateListFromFile), result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChangesList([FromRoute(Name = "id")] Guid listId,
            [FromBody] object command,
            CancellationToken cancellationToken)
        {

            return await Task.FromResult(Ok());
        }
    }
}
