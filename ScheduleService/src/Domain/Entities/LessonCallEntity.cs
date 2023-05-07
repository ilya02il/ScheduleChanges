using System;
using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Entities
{
    public class LessonCallEntity : BaseEntity
    {
        public int Position { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public long StartTime { get; private set; }
        public long EndTime { get; private set; }

        public Guid EducationalOrgId { get; private set; }
        public EducationalOrgEntity EducationalOrg { get; private set; }

        protected LessonCallEntity() : base() { }

        public LessonCallEntity(Guid educOrgId, int position, DayOfWeek dayOfWeek, long startTime, long endTime)
        {
            EducationalOrgId = educOrgId;
            Position = position;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }

        public void UpdateLessonCallInfo(int position, DayOfWeek dayOfWeek, long startTime, long endTime)
        {
            Position = position;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
