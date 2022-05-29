using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Contracts.v1.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; init; }
        [Required]
        public string RefreshToken { get; init; }
    }
}
