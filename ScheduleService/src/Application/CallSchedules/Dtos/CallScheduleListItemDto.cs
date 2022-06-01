using System;

namespace Application.CallSchedules.Dtos
{
    public class CallScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public long StartTime { get; init; }
        public long EndTime { get; init; }
    }
}
