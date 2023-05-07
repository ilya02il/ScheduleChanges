using System;

namespace WebAPI.Dtos.CallScheduleLists
{
    public class CreateCallScheduleListItemDto
    {
        public DayOfWeek DayOfWeek { get; init; }
        public int Position { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}
