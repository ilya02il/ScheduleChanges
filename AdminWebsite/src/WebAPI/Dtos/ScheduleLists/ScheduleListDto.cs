using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.ScheduleLists
{
    public class ScheduleListDto
    {
        public Guid Id { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<ScheduleListItemDto> ListItems { get; init; }
    }
}
