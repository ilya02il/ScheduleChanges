using System;

namespace WebAPI.Dtos.ScheduleLists
{
    public class ScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public bool? IsOddWeek { get; init; }
        public string Discipline { get; init; }
        public string Auditorium { get; init; }
        public string Teacher { get; init; }
    }
}
