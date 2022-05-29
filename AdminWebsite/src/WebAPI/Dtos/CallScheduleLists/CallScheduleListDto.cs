using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.CallScheduleLists
{
    public class CallScheduleListDto
    {
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<CallScheduleListItemDto> ListItems { get; init; }
    }
}
