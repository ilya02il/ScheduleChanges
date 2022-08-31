using System;

namespace Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery
{
    public record ScheduleWithChangesListItemDto
    {
        public int Position { get; init; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }
    }
}