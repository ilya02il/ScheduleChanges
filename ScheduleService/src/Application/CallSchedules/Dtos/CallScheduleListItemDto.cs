using System;

namespace Application.CallSchedules.Dtos
{
    public class CallScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public string StartTime { get; init; }
        public string EndTime { get; init; }
    }
}
