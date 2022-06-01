using Application.EducOrgs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI.Controllers
{
    [ApiController]
    [Route(ApiBaseRoute.BaseRoute + "/educational-orgs")]
    public class EducOrgsController : ControllerBase
    {
        private readonly ISender _sender;

        public EducOrgsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefEducOrgs(CancellationToken cancellationToken)
        {
            var query = new GetBriefEducOrgsQuery();
            var senderResponse = await _sender.Send(query, cancellationToken);

            return Ok(senderResponse);
        }
    }
}
