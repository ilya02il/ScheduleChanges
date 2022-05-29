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

        public RegisterRequest(Guid educOrgId, string username, string password)
        {
            EducOrgId = educOrgId;
            Username = username;
            Password = password;
        }
    }
}
