using System;

namespace Domain.Entities
{
    public class DatedLessonCallEntity : LessonCallEntity
    {
        public DateTimeOffset Date { get; private set; }

        private DatedLessonCallEntity() : base() { }

        public DatedLessonCallEntity(DateTimeOffset date)
        {
            Date = date;
        }
    }
}
