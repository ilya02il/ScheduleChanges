using System;

namespace WebAPI.Dtos.ChangesLists
{
    public class UpdateChangesListItemInfoDto
    {
        public Guid GroupId { get; init; }
        public int Position { get; init; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }
    }
}
