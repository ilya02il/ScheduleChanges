using System;
using System.Collections.Generic;

namespace Application.CallSchedules.Dtos
{
    public record CallScheduleListDto
    {
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<CallScheduleListItemDto> ListItems { get; init; }
    }
}
