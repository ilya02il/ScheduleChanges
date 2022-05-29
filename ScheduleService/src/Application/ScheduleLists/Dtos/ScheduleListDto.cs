using System;
using System.Collections.Generic;

namespace Application.ScheduleLists.Dtos
{
    public class ScheduleListDto
    {
        public Guid Id { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public IEnumerable<ScheduleListItemDto> ListItems { get; init; }
    }
}
