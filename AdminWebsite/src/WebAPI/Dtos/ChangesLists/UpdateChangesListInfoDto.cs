using System;

namespace WebAPI.Dtos.ChangesLists
{
    public class UpdateChangesListInfoDto
    {
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }
    }
}
