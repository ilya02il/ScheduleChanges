using System;

namespace WebAPI.Dtos.CallScheduleLists
{
    public class UpdateCallScheduleListItemDto
    {
        public int Position { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }

        public UpdateCallScheduleListItemDto(int position, TimeSpan startTime, TimeSpan endTime)
        {
            Position = position;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
