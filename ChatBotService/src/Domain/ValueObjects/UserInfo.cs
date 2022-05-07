using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.ValueObjects
{
    public class UserInfo : ValueObject
    {
        [Required(AllowEmptyStrings = true)]
        public string Username { get; set; }
        public EducationalInfo EducationalInfo { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Username;
            yield return EducationalInfo;
        }
    }
}
