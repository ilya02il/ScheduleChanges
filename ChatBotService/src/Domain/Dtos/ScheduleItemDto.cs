using System;

namespace Domain.Dtos
{
    public class ScheduleItemDto
    {
        public int Position { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }

        public ScheduleItemDto() { } 
    }
}