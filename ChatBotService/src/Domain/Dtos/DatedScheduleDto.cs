using System;
using System.Collections.Generic;

namespace Domain.Dtos
{
    public class DatedScheduleDto
    {
        public string EducOrgName { get; init; }
        public string GroupNumber { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset ExpirationTime { get; init; }
        public List<ScheduleItemDto> ScheduleItems { get; init; }
    }
}