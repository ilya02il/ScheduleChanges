using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityAPI.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid EducOrgId { get; set; }

        public ApplicationUser() : base() { }
        public ApplicationUser(string username, Guid educOrgId)
        {
            UserName = username;
            EducOrgId = educOrgId;
        }
    }
}
