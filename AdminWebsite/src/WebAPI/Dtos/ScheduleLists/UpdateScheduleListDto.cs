using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.ScheduleLists
{
    public class UpdateScheduleListDto
    {
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<ScheduleListItemDto> ListItems { get; init; }
    }
}
