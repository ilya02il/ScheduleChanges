using System;

namespace WebAPI.Dtos.CallScheduleLists
{
    public class CallScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public long StartTimeTicks { get; init; }
        public long EndTimeTicks { get; init; }
    }
}
