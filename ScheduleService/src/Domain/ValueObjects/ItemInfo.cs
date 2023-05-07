using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class ItemInfo : ValueObject
    {
        private int _position;
        private string _subjectName;
        private string _teacherInitials;
        private string _auditorium;

        public int Position
        {
            get => _position;
            set => _position = value > 0 ?
                    value :
                    throw new ArgumentException("The value of position property must be greater than zero.");
        }
        public string SubjectName
        {
            get => _subjectName;
            set => _subjectName = value ?? throw new ArgumentNullException(nameof(value));
        }
        public string TeacherInitials
        {
            get => _teacherInitials;
            set => _teacherInitials = value ?? throw new ArgumentNullException(nameof(value));
        }
        public string Auditorium
        {
            get => _auditorium;
            set => _auditorium = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Position;
            yield return SubjectName;
            yield return TeacherInitials;
            yield return Auditorium;
        }
    }
}
