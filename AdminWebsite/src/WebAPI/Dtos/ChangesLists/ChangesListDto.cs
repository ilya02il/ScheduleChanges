using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.ChangesLists
{
    public class ChangesListDto
    {
        public Guid Id { get; init; }
        public Guid EducationalOrgId { get; init; }
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }

        public List<ChangesListItemDto> ListItems { get; init; }
    }
}
