using System;

namespace Application.Groups.Dtos
{
    public class GroupDto
    {
        public Guid Id { get; init; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }
}
