using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityAPI.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalid { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
