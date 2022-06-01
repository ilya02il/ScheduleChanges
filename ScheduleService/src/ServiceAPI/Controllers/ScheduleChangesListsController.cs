using Application.ChangesLists.Commands.CreateChangesList;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Application.ChangesLists.Commands.DeleteChangesList;
using Application.ChangesLists.Commands.DeleteChangesListItem;
using Application.ChangesLists.Commands.UpdateChangesList;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using Application.ChangesLists.Queries.GetBriefScheduleChangesList;
using Application.ChangesLists.Queries.GetScheduleChangesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using ServiceAPI.Attributes;
using ServiceAPI.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route(ApiBaseRoute.BaseRoute + "/schedule-changes-lists")]
    public class ScheduleChangesListsController : ControllerBase
    {
        private const string EducOrgIdClaimType = "educ_org_id";
        private const long MaxFileSize = 100L * 1024L * 1024L;

        private readonly ISender _sender;

        public ScheduleChangesListsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetScheduleChangesListById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetChangesListByIdQuery(id);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefScheduleChangesListByEducOrgId([FromQuery] Guid educOrgId,
            CancellationToken cancellationToken)
        {
            if (educOrgId == Guid.Empty)
                BadRequest("The user claims don't contain an educational organization id claim.");

            var query = new GetBriefScheduleChangesListsQuery(educOrgId);
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleChangesList(CreateChangesListCommand command,
            CancellationToken cancellationToken)
        {
            command.EducOrgId = Guid.Parse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType));
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [HttpPost("from-file")]
        public async Task<IActionResult> CreateScheduleChangesListFromFile([FromQuery] DateTime date,
            [FromQuery] bool isOddWeek,
            CancellationToken cancellationToken)
        {
            var educOrgId = Guid.Parse(ClaimsHelper.GetClaimValueFromCurrentUserClaims(User, EducOrgIdClaimType));

            if (educOrgId == Guid.Empty)
                return BadRequest("The user claims don't contain an educational organization id claim.");

            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                return BadRequest("Not a multipart request");

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
            var reader = new MultipartReader(boundary, Request.Body);

            // note: this is for a single file, you could also process multiple files
            var section = await reader.ReadNextSectionAsync(cancellationToken);

            if (section == null)
                return BadRequest("No sections in multipart defined");

            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                return BadRequest("No content disposition in multipart defined");

            var fileName = contentDisposition.FileNameStar.ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = contentDisposition.FileName.ToString();
            }

            if (string.IsNullOrEmpty(fileName))
                return BadRequest("No filename defined.");

            if (Path.GetExtension(fileName) != ".docx")
                return new UnsupportedMediaTypeResult();

            return default;

            //var result = await _changesListsService.CreateScheduleChangesListFromFile(educOrgId,
            //    date,
            //    isOddWeek,
            //    section.Body,
            //    cancellationToken);

            //return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateScheduleChangesListInfo([FromRoute] Guid id,
            UpdateChangesListCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteScheduleChangesList([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteChangesListCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpPost("{listId:guid}/items")]
        public async Task<IActionResult> CreateScheduleChangesListItem([FromRoute] Guid listId,
            [FromBody] CreateChangesListItemCommand command,
            CancellationToken cancellationToken)
        {
            command.ListId = listId;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpPut("items/{id:guid}")]
        public async Task<IActionResult> UpdateScheduleChangesListItem([FromRoute] Guid id,
            UpdateChangesListItemCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }

        [HttpDelete("items/{id:guid}")]
        public async Task<IActionResult> DeleteScheduleChangesListItem([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteChangesListItemCommand(id);
            var senderResponse = await _sender.Send(command, cancellationToken);

            if (!senderResponse)
                return BadRequest();

            return Ok(senderResponse);
        }
    }
}
