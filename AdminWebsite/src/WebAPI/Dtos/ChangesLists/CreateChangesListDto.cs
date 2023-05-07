using System;

namespace WebAPI.Dtos.ChangesLists
{
    public class CreateChangesListDto
    {
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }
    }
}
