using System;
using System.Collections.Generic;

namespace Application.CallSchedules.Dtos
{
    public class CallScheduleListDto
    {
        public Guid Id { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<CallScheduleListItemDto> ListItems { get; init; }
    }
}
