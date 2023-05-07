using Domain.Common;
using System.Collections.Generic;

namespace Domain.ValueObjects
{
    public class ScheduleItemInfo : ValueObject
    {
        public string Position { get; set; }
        public string Discipline { get; set; }
        public string Teacher { get; set; }
        public string Auditorium { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Position;
            yield return Discipline;
            yield return Teacher;
            yield return Auditorium;
        }
    }
}
