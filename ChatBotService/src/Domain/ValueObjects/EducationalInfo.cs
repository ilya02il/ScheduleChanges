using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.ValueObjects
{
    public class EducationalInfo : BaseValueObject<EducationalInfo>
    {
        public string EducOrgName { get; set; }
        public int YearOfStudy { get; set; }
        public string GroupNumber { get; set; }
    }
}
