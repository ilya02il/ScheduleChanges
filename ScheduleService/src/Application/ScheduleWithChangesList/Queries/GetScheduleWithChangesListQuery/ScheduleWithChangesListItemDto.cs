using System;

namespace Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery
{
    public record ScheduleWithChangesListItemDto
    {
        public int Position { get; init; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }
    }
}