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
    [Route(ApiBaseRoute.BaseRoute + "/account")]
    [Authorize(Roles = "Admin")]
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

            foreach (var (key, value, options) in authResult.Cookies)
            {
                Response.Cookies.Append(key, value, options);
            }

            return Ok(new AuthSuccessResponse(authResult.Token));
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
                request.Role,
                cancellationToken);

            if (!registerResult.IsSuccess)
                return BadRequest(new FailResponse(registerResult.Errors));

            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            if (!ValidateModel(out var result))
                return result;

            var refreshToken = Request.Cookies
                .FirstOrDefault(c => c.Key == "X-Refresh-Token")
                .Value;

            var refreshResult = await _identityService.RefreshTokenAsync(GetTokenFromAuthHeader(),
                refreshToken,
                cancellationToken);

            if (!refreshResult.IsSuccess)
                return BadRequest(new FailResponse(refreshResult.Errors));

            foreach (var (key, value, options) in refreshResult.Cookies)
            {
                Response.Cookies.Append(key, value, options);
            }

            return Ok(new AuthSuccessResponse(refreshResult.Token));
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

        private string GetTokenFromAuthHeader()
        {
            return HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()[7..];
        }
    }
}
