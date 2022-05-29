using System;

namespace Application.CallSchedules.Dtos
{
    public class CallScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }

        public CallScheduleListItemDto(int position, TimeSpan startTime, TimeSpan endTime)
        {
            Position = position;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
