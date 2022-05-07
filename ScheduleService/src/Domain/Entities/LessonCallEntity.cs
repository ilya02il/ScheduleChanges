using Domain.Common;
using System;

namespace Domain.Entities
{
    public class LessonCallEntity : Entity
    {
        public int Position { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public Guid EducationalOrgId { get; private set; }
        public EducationalOrgEntity EducationalOrg { get; private set; }

        protected LessonCallEntity() : base() { }

        public LessonCallEntity(Guid educOrgId, int position, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            EducationalOrgId = educOrgId;
            Position = position;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
