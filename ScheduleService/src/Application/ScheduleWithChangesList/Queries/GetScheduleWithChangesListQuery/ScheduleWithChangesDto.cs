using System;
using System.Collections.Generic;

namespace Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery
{
    public record ScheduleWithChangesDto
    {
        public string EducOrgName { get; init; }
        public string GroupNumber { get; init; }
        public DateTimeOffset Date { get; init; }

        public IReadOnlyList<ScheduleWithChangesListItemDto> ScheduleItems { get; init; }
    }
}