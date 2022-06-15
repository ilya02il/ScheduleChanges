using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Contracts.v1.Requests
{
    public class RegisterRequest
    {
        public Guid EducOrgId { get; init; }
        [Required]
        public string Username { get; init; }
        [Required]
        public string Password { get; init; }
        [Required]
        public string Role { get; init; }
    }
}
