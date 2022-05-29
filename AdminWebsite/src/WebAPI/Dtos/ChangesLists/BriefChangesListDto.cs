using System;

namespace WebAPI.Dtos.ChangesLists
{
    public class BriefChangesListDto
    {
        public Guid Id { get; init; }
        public DateTimeOffset Date { get; init; }
    }
}
