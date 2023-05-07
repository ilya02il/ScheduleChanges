using System.ComponentModel.DataAnnotations;

using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.ValueObjects
{
    public class UserInfo : BaseValueObject<UserInfo>
    {
        [Required(AllowEmptyStrings = true)]
        public string Username { get; set; }
        public EducationalInfo EducationalInfo { get; set; }
    }
}
