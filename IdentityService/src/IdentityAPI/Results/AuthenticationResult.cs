using System.Collections.Generic;

namespace IdentityAPI.Results
{
    public class AuthenticationResult
    {
        public string Token { get; init; }
        public bool IsSuccess { get; init; } = false;
        public string RefreshToken { get; init; }
        public IEnumerable<string> Errors { get; init; }

        public AuthenticationResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public AuthenticationResult(bool isSuccess, string token, string refreshToken)
        {
            IsSuccess = isSuccess;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
