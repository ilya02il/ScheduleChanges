using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IdentityAPI.Results
{
    public class AuthenticationResult
    {
        public string Token { get; init; }
        public bool IsSuccess { get; init; } = false;
        public IEnumerable<(string key, string value, CookieOptions options)> Cookies { get; set; }
        public IEnumerable<string> Errors { get; init; }

        public AuthenticationResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public AuthenticationResult(bool isSuccess,
            string token,
            IEnumerable<(string key, string value, CookieOptions options)> cookies)
        {
            IsSuccess = isSuccess;
            Token = token;
            Cookies = cookies;
        }
    }
}
