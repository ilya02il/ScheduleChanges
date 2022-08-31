using System;
using System.Collections.Generic;

namespace Application.ScheduleLists.Dtos
{
    public class ScheduleListDto
    {
        public Guid Id { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public IList<ScheduleListItemDto> ListItems { get; init; } = new List<ScheduleListItemDto>();
    }
}
