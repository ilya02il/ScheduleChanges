using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.ValueObjects
{
    public class ScheduleItemInfo : BaseValueObject<ScheduleItemInfo>
    {
        public string Position { get; set; }
        public string Discipline { get; set; }
        public string Teacher { get; set; }
        public string Auditorium { get; set; }
    }
}
