using IdentityAPI.Contracts;
using IdentityAPI.Contracts.v1.Requests;
using IdentityAPI.Contracts.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityAPI.Controllers
{
    [ApiController]
    [Route("identity-api/v1/account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            if (!ValidateModel(out var result))
                return result;

            var authResult = await _identityService.LoginAsync(request.Username,
                request.Password,
                cancellationToken);

            if (!authResult.IsSuccess)
                return BadRequest(new FailResponse(authResult.Errors));

            return Ok(new AuthSuccessResponse(authResult.Token, authResult.RefreshToken));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            if (!ValidateModel(out var result))
                return result;

            var registerResult = await _identityService.RegisterUserAsync(request.EducOrgId,
                request.Username,
                request.Password,
                cancellationToken);

            if (!registerResult.IsSuccess)
                return BadRequest(new FailResponse(registerResult.Errors));

            return Ok();
        }

        [HttpPost("resfresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            if (!ValidateModel(out var result))
                return result;

            var refreshResult = await _identityService.RefreshTokenAsync(request.Token,
                request.RefreshToken,
                cancellationToken);

            return Ok(new AuthSuccessResponse(refreshResult.Token, refreshResult.RefreshToken));
        }

        private bool ValidateModel(out IActionResult result)
        {
            if (!ModelState.IsValid)
            {
                var response = new FailResponse(ModelState.Values
                    .SelectMany(val => val.Errors.Select(err => err.ErrorMessage)));

                result = BadRequest(response);
                return false;
            }

            result = null;
            return true;
        }
    }
}
