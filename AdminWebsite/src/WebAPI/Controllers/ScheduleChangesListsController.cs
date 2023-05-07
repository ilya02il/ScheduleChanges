using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.Dtos.ChangesLists;
using WebAPI.Helpers;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("schedule-changes-lists")]
    public class ScheduleChangesListsController : ControllerBase
    {
        private const long MaxFileSize = 100L * 1024L * 1024L;

        private readonly GrpcScheduleChangesListsClientService _changesListsService;

        public ScheduleChangesListsController(GrpcScheduleChangesListsClientService changesListsService)
        {
            _changesListsService = changesListsService;
        }

        [HttpGet("{id:guid}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetScheduleChangesListById([FromRoute] Guid id,//delete this when the identity server will be created
            CancellationToken cancellationToken)
        {
            //var educOrgId = GetEducOrgIdFromClaims();//uncomment this

            if (id == Guid.Empty)
                BadRequest("The user claims don't contain an educational organization id claim.");

            var result = await _changesListsService.GetScheduleChangesListById(id,
                cancellationToken);

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("brief")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetBriefScheduleChangesListByEducOrgId([FromQuery] Guid educOrgId,//delete this when the identity server will be created
            CancellationToken cancellationToken)
        {
            //var educOrgId = GetEducOrgIdFromClaims();//uncomment this

            if (educOrgId == Guid.Empty)
                BadRequest("The user claims don't contain an educational organization id claim.");

            var result = await _changesListsService.GetBriefScheduleChangesListByEducOrgId(educOrgId,
                cancellationToken);

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleChangesList([FromQuery] Guid educOrgId,//delete this when the identity service will be created
            CreateChangesListDto changesList,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.CreateScheduleChangesList(educOrgId,
                changesList,
                cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [HttpPost("from-file")]
        public async Task<IActionResult> CreateScheduleChangesListFromFile([FromQuery] Guid educOrgId,//delete this when the identity server will be created
            [FromQuery] DateTime date,
            [FromQuery] bool isOddWeek,
            CancellationToken cancellationToken)
        {
            //var educOrgId = GetEducOrgIdFromClaims();//uncomment this

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

            var result = await _changesListsService.CreateScheduleChangesListFromFile(educOrgId,
                date,
                isOddWeek,
                section.Body,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateScheduleChangesListInfo([FromRoute] Guid id,
            UpdateChangesListInfoDto changesListInfo,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.UpdateScheduleChangesList(id, 
                changesListInfo,
                cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteScheduleChangesList([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.DeleteScheduleChangesList(id, cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpPost("{listId:guid}/items")]
        public async Task<IActionResult> CreateScheduleChangesListItem([FromRoute] Guid listId,
            [FromBody] CreateChangesListItemDto changesListItem,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.CreateScheduleChangesListItem(listId,
                changesListItem,
                cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut("items/{id:guid}")]
        public async Task<IActionResult> UpdateScheduleChangesListItem([FromRoute] Guid id,
            UpdateChangesListItemInfoDto changesListItemInfo,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.UpdateScheduleChangesListItem(id,
                changesListItemInfo,
                cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete("items/{id:guid}")]
        public async Task<IActionResult> DeleteScheduleChangesListItem([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _changesListsService.DeleteScheduleChangesListItem(id, cancellationToken);

            if (!result)
                return BadRequest();

            return Ok(result);
        }

        private Guid GetEducOrgIdFromClaims()
        {
            var claimValue = HttpContext.User.Claims.First(c => c.Type == "educOrgId").Value;//TODO: change this when the identity service will be created

            if (!Guid.TryParse(claimValue, out Guid result))
                return Guid.Empty;

            return result;
        }
    }
}
