using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Contracts.v1.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
