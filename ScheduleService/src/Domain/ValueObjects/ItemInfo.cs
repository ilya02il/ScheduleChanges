using Domain.Common;
using System.Collections.Generic;

namespace Domain.ValueObjects
{
    public class ItemInfo : ValueObject
    {
        public int Position { get; set; }
        public string SubjectName { get; set; }
        public string TeacherInitials { get; set; }
        public string Auditorium { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Position;
            yield return SubjectName;
            yield return TeacherInitials;
            yield return Auditorium;
        }
    }
}
